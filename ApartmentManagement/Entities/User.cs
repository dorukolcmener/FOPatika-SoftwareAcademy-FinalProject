using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string EMail { get; set; }
    public string Phone { get; set; }
    public long TCNo { get; set; }
    public string Password { get; set; }
    public UserType Role { get; set; }
    public double Balance { get; set; }
}

public enum UserType
{
    admin = 0,
    owner,
    renter
}