using AutoFixture;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;
using SalonManagement.Services.Services;
using Xunit;

namespace SalonManagement.Services.Tests.Services;

public class AppointmentsServiceTests : TestBase
{
    private readonly IAppointmentsService _service;

    public AppointmentsServiceTests()
    {
        _service = new AppointmentsService(DbContext, Mapper);
    }

    [Fact]
    public async Task GetAllAppointmentsAsync_ShouldReturnMappedAppointments()
    {
        // Arrange
        var category = Fixture.Create<ServiceCategory>();
        var service = Fixture.Build<Service>()
            .With(s => s.Category, category)
            .With(s => s.CategoryId, category.Id)
            .Create();

        var customer = Fixture.Create<Customer>();

        var appointments = Fixture.Build<Appointment>()
            .With(a => a.Customer, customer)
            .With(a => a.CustomerId, customer.Id)
            .With(a => a.Service, service)
            .With(a => a.ServiceId, service.Id)
            .CreateMany(2)
            .AsQueryable()
            .BuildMockDbSet();

        DbContext.Appointments.Returns(appointments);

        // Act
        var result = (await _service.GetAllAppointmentsAsync()).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(a => a.Customer?.Id == customer.Id).Should().BeTrue();
        result.All(a => a.Service?.Id == service.Id).Should().BeTrue();
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_WhenAppointmentExists_ShouldReturnMappedAppointment()
    {
        // Arrange
        var category = Fixture.Create<ServiceCategory>();
        var service = Fixture.Build<Service>()
            .With(s => s.Category, category)
            .With(s => s.CategoryId, category.Id)
            .Create();

        var customer = Fixture.Create<Customer>();
        var appointment = Fixture.Build<Appointment>()
            .With(a => a.Customer, customer)
            .With(a => a.CustomerId, customer.Id)
            .With(a => a.Service, service)
            .With(a => a.ServiceId, service.Id)
            .Create();

        var mockSet = new List<Appointment> { appointment }
            .AsQueryable()
            .BuildMockDbSet();

        DbContext.Appointments.Returns(mockSet);

        // Act
        var result = await _service.GetAppointmentByIdAsync(appointment.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(appointment.Id);
        result.Customer.Should().NotBeNull();
        result.Customer!.Id.Should().Be(customer.Id);
        result.Service.Should().NotBeNull();
        result.Service!.Id.Should().Be(service.Id);
        result.Service.Category.Should().NotBeNull();
        result.Service.Category!.Id.Should().Be(category.Id);
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_WhenAppointmentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var mockSet = new List<Appointment>()
            .AsQueryable()
            .BuildMockDbSet();

        DbContext.Appointments.Returns(mockSet);

        // Act
        var result = await _service.GetAppointmentByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }
}
