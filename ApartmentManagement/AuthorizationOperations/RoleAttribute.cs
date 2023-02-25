using ApartmentManagement.Entities;

namespace ApartmentManagement.AuthorizationOperations;

public class RoleAttribute : Attribute
{
    public int Role { get; set; }
    public RoleAttribute(string role)
    {
        if (role == "admin")
            // Set enum admin
            Role = (int)UserType.admin;
    }
    public RoleAttribute()
    {
        Role = (int)UserType.renter;
    }
}