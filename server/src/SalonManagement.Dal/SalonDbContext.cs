using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Dal;

public class SalonDbContext : DbContext, ISalonDbContext
{
    public SalonDbContext(DbContextOptions<SalonDbContext> options)
        : base(options) { }

    public virtual DbSet<Appointment> Appointments => Set<Appointment>();
    public virtual DbSet<Customer> Customers => Set<Customer>();
    public virtual DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

    public DbSet<Package> Packages { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<ServicePackage> ServicePackages { get; set; }
}
