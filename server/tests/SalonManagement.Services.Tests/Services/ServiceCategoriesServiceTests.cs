using AutoFixture;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;
using SalonManagement.Services.Services;
using Xunit;

namespace SalonManagement.Services.Tests.Services;

public class ServiceCategoriesServiceTests : TestBase
{
    private readonly IServiceCategoriesService _service;

    public ServiceCategoriesServiceTests()
    {
        _service = new ServiceCategoriesService(DbContext, Mapper);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnMappedCategories()
    {
        // Arrange
        var categories = Fixture.CreateMany<ServiceCategory>(3).ToList();
        var mockDbSet = categories.AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockDbSet);

        // Act
        var result = (await _service.GetAllCategoriesAsync()).ToList();

        // Assert
        result.Should().HaveCount(3);
        result.Select(c => c.Title).Should().BeEquivalentTo(categories.Select(c => c.Title));
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ShouldReturnMappedCategory()
    {
        // Arrange
        var category = Fixture.Create<ServiceCategory>();
        var categories = new List<ServiceCategory> { category };
        var mockDbSet = categories.AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockDbSet);

        // Act
        var result = await _service.GetCategoryByIdAsync(category.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(category.Id);
        result.Title.Should().Be(category.Title);
        result.Description.Should().Be(category.Description);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockDbSet = new List<ServiceCategory>().AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockDbSet);

        // Act
        var result = await _service.GetCategoryByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCategoryAsync_ShouldCreateCategory()
    {
        // Arrange
        var categoryDto = Fixture.Create<ServiceCategoryDto>();
        var categories = new List<ServiceCategory>();
        var mockDbSet = categories.AsQueryable().BuildMockDbSet();

        mockDbSet.When(x => x.Add(Arg.Any<ServiceCategory>()))
            .Do(callInfo => 
            {
                var category = callInfo.Arg<ServiceCategory>();
                category.Id = 1;
                categories.Add(category);
            });

        DbContext.ServiceCategories.Returns(mockDbSet);
        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _service.CreateCategoryAsync(categoryDto);

        // Assert
        mockDbSet.Received(1).Add(Arg.Any<ServiceCategory>());
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        
        result.Should().NotBeNull();
        result.Title.Should().Be(categoryDto.Title);
        result.Description.Should().Be(categoryDto.Description);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryExists_ShouldUpdateCategory()
    {
        // Arrange
        var existingCategory = Fixture.Create<ServiceCategory>();
        var categoryDto = Fixture.Build<ServiceCategoryDto>()
            .With(c => c.Id, existingCategory.Id)
            .With(c => c.Title, "Updated Title")
            .With(c => c.Description, "Updated Description")
            .Create();

        var categories = new List<ServiceCategory> { existingCategory };
        var mockDbSet = categories.AsQueryable().BuildMockDbSet();

        mockDbSet.FindAsync(existingCategory.Id)
            .Returns(new ValueTask<ServiceCategory?>(existingCategory));

        DbContext.ServiceCategories.Returns(mockDbSet);
        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        await _service.UpdateCategoryAsync(existingCategory.Id, categoryDto);

        // Assert
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        existingCategory.Title.Should().Be(categoryDto.Title);
        existingCategory.Description.Should().Be(categoryDto.Description);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var categoryDto = Fixture.Create<ServiceCategoryDto>();
        var mockDbSet = new List<ServiceCategory>().AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockDbSet);

        // Act
        var act = () => _service.UpdateCategoryAsync(1, categoryDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Service category with ID 1 not found.");
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryHasNoServices_ShouldDeleteCategory()
    {
        // Arrange
        var category = Fixture.Create<ServiceCategory>();
        var categories = new List<ServiceCategory> { category };
        var mockDbSet = categories.AsQueryable().BuildMockDbSet();

        mockDbSet.FindAsync(category.Id)
            .Returns(new ValueTask<ServiceCategory?>(category));

        DbContext.ServiceCategories.Returns(mockDbSet);

        var services = new List<Service>();
        var mockServicesDbSet = services.AsQueryable().BuildMockDbSet();
        DbContext.Services.Returns(mockServicesDbSet);

        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        await _service.DeleteCategoryAsync(category.Id);

        // Assert
        mockDbSet.Received(1).Remove(category);
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryHasServices_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var category = Fixture.Create<ServiceCategory>();
        var categories = new List<ServiceCategory> { category };
        var mockDbSet = categories.AsQueryable().BuildMockDbSet();

        mockDbSet.FindAsync(category.Id)
            .Returns(new ValueTask<ServiceCategory?>(category));

        DbContext.ServiceCategories.Returns(mockDbSet);

        var services = new List<Service> 
        { 
            new() { CategoryId = category.Id }
        };
        var mockServicesDbSet = services.AsQueryable().BuildMockDbSet();
        DbContext.Services.Returns(mockServicesDbSet);

        // Act
        var act = () => _service.DeleteCategoryAsync(category.Id);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Cannot delete category with ID {category.Id} because it has associated services.");
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var mockDbSet = new List<ServiceCategory>().AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockDbSet);

        // Act
        var act = () => _service.DeleteCategoryAsync(1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Service category with ID 1 not found.");
    }
} 