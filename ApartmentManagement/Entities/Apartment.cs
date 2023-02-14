using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities;

public class Apartment
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Block { get; set; }
    public int Floor { get; set; }
    public int Door { get; set; }
    public string FlatType { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
}