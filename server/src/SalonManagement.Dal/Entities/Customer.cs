using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonManagement.Dal.Entities;

[Table("customers")]
public class Customer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; }

    [Column("loyalty_points")]
    public int LoyaltyPoints { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
}