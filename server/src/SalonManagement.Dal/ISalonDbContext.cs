using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Dal;

public interface ISalonDbContext
{
    DbSet<Appointment> Appointments { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Service> Services { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
