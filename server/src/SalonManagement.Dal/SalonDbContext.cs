using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Dal;

public class SalonDbContext : DbContext
{
    public SalonDbContext(DbContextOptions<SalonDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema for all entities
        modelBuilder.HasDefaultSchema("master");

        // Configure ServicePackage as a many-to-many relationship
        modelBuilder.Entity<ServicePackage>()
            .HasKey(sp => new { sp.PackageId, sp.ServiceId });

        modelBuilder.Entity<ServicePackage>()
            .HasOne(sp => sp.Package)
            .WithMany(p => p.ServicePackages)
            .HasForeignKey(sp => sp.PackageId);

        modelBuilder.Entity<ServicePackage>()
            .HasOne(sp => sp.Service)
            .WithMany(s => s.ServicePackages)
            .HasForeignKey(sp => sp.ServiceId);
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<ServicePackage> ServicePackages { get; set; }
    public DbSet<Service> Services { get; set; }
}