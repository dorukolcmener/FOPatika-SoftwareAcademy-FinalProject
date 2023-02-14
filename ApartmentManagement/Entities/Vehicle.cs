using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities;

public class Vehicle
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string LicensePlate { get; set; }
    public User Owner { get; set; }
    public int OwnerId { get; set; }
}