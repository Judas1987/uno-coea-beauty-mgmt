using System.ComponentModel.DataAnnotations.Schema;

namespace SalonManagement.Dal.Entities;

[Table("service_packages")]
public class ServicePackage
{
    [Column("package_id")]
    public int PackageId { get; set; }

    [Column("service_id")]
    public int ServiceId { get; set; }

    [ForeignKey(nameof(PackageId))]
    public Package Package { get; set; }

    [ForeignKey(nameof(ServiceId))]
    public Service Service { get; set; }
}