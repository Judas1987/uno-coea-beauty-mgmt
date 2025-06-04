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

public class ServicesServiceTests : TestBase
{
    private readonly IServicesService _service;

    public ServicesServiceTests()
    {
        _service = new ServicesService(DbContext, Mapper);
    }

    [Fact]
    public async Task GetAllServicesAsync_ShouldReturnMappedServices()
    {
        // Arrange
        var services = Fixture.CreateMany<Service>(3).ToList();
        var mockDbSet = services.AsQueryable().BuildMockDbSet();
        DbContext.Services.Returns(mockDbSet);

        // Act
        var result = (await _service.GetAllServicesAsync()).ToList();

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetServiceByIdAsync_WhenServiceExists_ShouldReturnMappedService()
    {
        // Arrange
        var service = Fixture.Create<Service>();
        var services = new List<Service> { service };
        var mockDbSet = services.AsQueryable().BuildMockDbSet();
        DbContext.Services.Returns(mockDbSet);

        // Act
        var result = await _service.GetServiceByIdAsync(service.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(service.Id);
        result.Title.Should().Be(service.Title);
        result.Description.Should().Be(service.Description);
        result.Price.Should().Be(service.Price);
        result.DurationMinutes.Should().Be(service.DurationMinutes);
    }

    [Fact]
    public async Task GetServiceByIdAsync_WhenServiceDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockDbSet = new List<Service>().AsQueryable().BuildMockDbSet();
        DbContext.Services.Returns(mockDbSet);

        // Act
        var result = await _service.GetServiceByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateServiceAsync_ShouldCreateServiceWithDefaultValues()
    {
        // Arrange
        var serviceDto = Fixture.Create<ServiceDto>();
        var services = new List<Service>();
        var mockDbSet = services.AsQueryable().BuildMockDbSet();

        mockDbSet.When(x => x.Add(Arg.Any<Service>()))
            .Do(callInfo =>
            {
                var service = callInfo.Arg<Service>();
                service.Id = 1;
                services.Add(service);
            });

        DbContext.Services.Returns(mockDbSet);

        // Setup category validation
        var categories = new List<ServiceCategory>
        {
            new() { Id = serviceDto.CategoryId }
        };
        var mockCategoriesDbSet = categories.AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockCategoriesDbSet);

        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _service.CreateServiceAsync(serviceDto);

        // Assert
        mockDbSet.Received(1).Add(Arg.Any<Service>());
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        result.Should().NotBeNull();
        result.Title.Should().Be(serviceDto.Title);
        result.Description.Should().Be(serviceDto.Description);
        result.Price.Should().Be(serviceDto.Price);
        result.DurationMinutes.Should().Be(serviceDto.DurationMinutes);
        result.IsActive.Should().BeTrue();
        result.IsPromotional.Should().BeFalse();
        result.PromotionalPrice.Should().BeNull();
    }

    [Fact]
    public async Task CreateServiceAsync_WithInvalidCategory_ShouldThrowArgumentException()
    {
        // Arrange
        var serviceDto = Fixture.Create<ServiceDto>();
        var categories = new List<ServiceCategory>();
        var mockCategoriesDbSet = categories.AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockCategoriesDbSet);

        // Act
        var act = () => _service.CreateServiceAsync(serviceDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Service category with ID {serviceDto.CategoryId} not found.");
    }

    [Fact]
    public async Task UpdateServiceAsync_WhenServiceExists_ShouldUpdateService()
    {
        // Arrange
        var existingService = Fixture.Create<Service>();
        var serviceDto = Fixture.Build<ServiceDto>()
            .With(s => s.Id, existingService.Id)
            .With(s => s.Title, "Updated Title")
            .With(s => s.Description, "Updated Description")
            .Create();

        var services = new List<Service> { existingService };
        var mockDbSet = services.AsQueryable().BuildMockDbSet();

        mockDbSet.FindAsync(existingService.Id)
            .Returns(new ValueTask<Service?>(existingService));

        DbContext.Services.Returns(mockDbSet);

        // Setup category validation
        var categories = new List<ServiceCategory>
        {
            new() { Id = serviceDto.CategoryId }
        };
        var mockCategoriesDbSet = categories.AsQueryable().BuildMockDbSet();
        DbContext.ServiceCategories.Returns(mockCategoriesDbSet);

        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        await _service.UpdateServiceAsync(existingService.Id, serviceDto);

        // Assert
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        existingService.Title.Should().Be(serviceDto.Title);
        existingService.Description.Should().Be(serviceDto.Description);
    }

    [Fact]
    public async Task UpdateServiceAsync_WhenServiceDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var serviceDto = Fixture.Create<ServiceDto>();
        var mockDbSet = new List<Service>().AsQueryable().BuildMockDbSet();
        DbContext.Services.Returns(mockDbSet);

        // Act
        var act = () => _service.UpdateServiceAsync(1, serviceDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Service with ID 1 not found.");
    }

    [Fact]
    public async Task DeleteServiceAsync_WhenServiceHasNoAppointments_ShouldRemoveService()
    {
        // Arrange
        var service = Fixture.Create<Service>();
        var services = new List<Service> { service };
        var mockDbSet = services.AsQueryable().BuildMockDbSet();

        mockDbSet.FindAsync(service.Id)
            .Returns(new ValueTask<Service?>(service));

        DbContext.Services.Returns(mockDbSet);

        var appointments = new List<Appointment>();
        var mockAppointmentsDbSet = appointments.AsQueryable().BuildMockDbSet();
        DbContext.Appointments.Returns(mockAppointmentsDbSet);

        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        await _service.DeleteServiceAsync(service.Id);

        // Assert
        mockDbSet.Received(1).Remove(service);
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
