namespace ApartmentManagement.Models;

public class UserCreateModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
    public long TCNo { get; set; }
    public double Balance { get; set; }
}