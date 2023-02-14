using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.DBOperations;

public class ApartmentDBContext : DbContext
{
    // Implement context
    public ApartmentDBContext(DbContextOptions<ApartmentDBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Bill> Bills { get; set; }
}