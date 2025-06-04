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

public class CustomersServiceTests : TestBase
{
    private readonly ICustomersService _service;

    public CustomersServiceTests()
    {
        _service = new CustomersService(DbContext, Mapper);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ShouldReturnMappedCustomers()
    {
        // Arrange
        var customers = Fixture.CreateMany<Customer>(3)
            .AsQueryable()
            .BuildMockDbSet();

        DbContext.Customers.Returns(customers);

        // Act
        var result = (await _service.GetAllCustomersAsync()).ToList();

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerExists_ShouldReturnMappedCustomer()
    {
        // Arrange
        var appointments = Fixture.CreateMany<Appointment>(2).ToList();
        var customer = Fixture.Build<Customer>()
            .With(c => c.Appointments, appointments)
            .Create();

        var mockSet = new List<Customer> { customer }
            .AsQueryable()
            .BuildMockDbSet();

        DbContext.Customers.Returns(mockSet);

        // Act
        var result = await _service.GetCustomerByIdAsync(customer.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customer.Id);
        result.FirstName.Should().Be(customer.FirstName);
        result.LastName.Should().Be(customer.LastName);
        result.Email.Should().Be(customer.Email);
        result.PhoneNumber.Should().Be(customer.PhoneNumber);
        result.LoyaltyPoints.Should().Be(customer.LoyaltyPoints);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockSet = new List<Customer>()
            .AsQueryable()
            .BuildMockDbSet();

        DbContext.Customers.Returns(mockSet);

        // Act
        var result = await _service.GetCustomerByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCustomerAsync_ShouldCreateCustomer()
    {
        // Arrange
        var customerDto = Fixture.Build<CustomerDto>()
            .Create();

        var savedCustomer = Fixture.Build<Customer>()
            .With(c => c.Id, 1)
            .With(c => c.FirstName, customerDto.FirstName)
            .With(c => c.LastName, customerDto.LastName)
            .With(c => c.Email, customerDto.Email)
            .With(c => c.PhoneNumber, customerDto.PhoneNumber)
            .With(c => c.LoyaltyPoints, 0)
            .Create();

        var customers = new List<Customer>();
        var mockDbSet = customers.AsQueryable().BuildMockDbSet();

        mockDbSet.When(x => x.Add(Arg.Any<Customer>()))
            .Do(callInfo =>
            {
                var customer = callInfo.Arg<Customer>();
                customer.Id = savedCustomer.Id;
                customer.LoyaltyPoints = 0;
                customers.Add(customer);
            });

        DbContext.Customers.Returns(mockDbSet);
        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _service.CreateCustomerAsync(customerDto);

        // Assert
        mockDbSet.Received(1).Add(Arg.Any<Customer>());
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        result.Should().NotBeNull();
        result.Id.Should().Be(savedCustomer.Id);
        result.FirstName.Should().Be(customerDto.FirstName);
        result.LastName.Should().Be(customerDto.LastName);
        result.Email.Should().Be(customerDto.Email);
        result.PhoneNumber.Should().Be(customerDto.PhoneNumber);
        result.LoyaltyPoints.Should().Be(0);
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenCustomerExists_ShouldUpdateCustomer()
    {
        // Arrange
        var existingCustomer = Fixture.Build<Customer>()
            .With(c => c.Id, 1)
            .Create();

        var customerDto = Fixture.Build<CustomerDto>()
            .With(c => c.Id, existingCustomer.Id)
            .With(c => c.FirstName, "UpdatedFirstName")
            .With(c => c.LastName, "UpdatedLastName")
            .With(c => c.Email, "updated@email.com")
            .Create();

        var customers = new List<Customer> { existingCustomer };
        var mockDbSet = customers.AsQueryable().BuildMockDbSet();

        mockDbSet.FindAsync(existingCustomer.Id)
            .Returns(new ValueTask<Customer?>(existingCustomer));

        DbContext.Customers.Returns(mockDbSet);
        DbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        await _service.UpdateCustomerAsync(existingCustomer.Id, customerDto);

        // Assert
        await DbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        existingCustomer.FirstName.Should().Be(customerDto.FirstName);
        existingCustomer.LastName.Should().Be(customerDto.LastName);
        existingCustomer.Email.Should().Be(customerDto.Email);
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenCustomerDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var customerDto = Fixture.Create<CustomerDto>();
        var mockSet = new List<Customer>().AsQueryable().BuildMockDbSet();
        DbContext.Customers.Returns(mockSet);

        // Act
        var act = () => _service.UpdateCustomerAsync(1, customerDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Customer with ID 1 not found.");
    }
}
