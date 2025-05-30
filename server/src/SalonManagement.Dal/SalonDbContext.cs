using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Dal;

public class SalonDbContext(DbContextOptions<SalonDbContext> options) : DbContext(options)
{
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<ServicePackage> ServicePackages { get; set; }
    public DbSet<Service> Services { get; set; }
}