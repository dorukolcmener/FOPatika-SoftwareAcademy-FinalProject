using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.DBOperations;

public class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApartmentDBContext(serviceProvider.GetRequiredService<DbContextOptions<ApartmentDBContext>>()))
        {
            if (context.Users.Any())
            {
                return;
            }

            context.Users.AddRange(
                new User { Name = "Touka", Surname = "Kirishima", EMail = "touka.kirishima@test.com", Phone = "+1231231223", TCNo = 12345678901, Role = UserType.admin },
                new User { Name = "Avatar", Surname = "Korra", EMail = "avatar.korra@test.com", Phone = "+44231231223", TCNo = 12345678901, Role = UserType.owner }
            );

            context.SaveChanges();

            context.Vehicles.AddRange(
                new Vehicle { LicensePlate = "34 ABC 34", OwnerId = context.Users.First().Id },
                new Vehicle { LicensePlate = "34 ABC 35", OwnerId = context.Users.First().Id },
                new Vehicle { LicensePlate = "34 ABC 36", OwnerId = context.Users.First().Id }
            );

            context.Apartments.AddRange(
                new Apartment { Block = "A Block", Floor = 1, Door = 1, UserId = context.Users.First().Id, FlatType = "1+1" },
                new Apartment { Block = "A Block", Floor = 1, Door = 2, UserId = context.Users.First().Id, FlatType = "2+1" },
                new Apartment { Block = "A Block", Floor = 1, Door = 3, UserId = context.Users.First().Id, FlatType = "3+1" }
            );

            context.SaveChanges();

            context.Bills.AddRange(
                new Bill { ApartmentId = context.Apartments.First().Id, Amount = 100, DueDate = DateTime.Now.AddDays(5).ToUniversalTime(), IsPaid = true, Type = BillType.monthly },
                new Bill { ApartmentId = context.Apartments.First().Id, Amount = 100, DueDate = DateTime.Now.AddDays(15).ToUniversalTime(), IsPaid = false, Type = BillType.electricity },
                new Bill { ApartmentId = context.Apartments.First().Id, Amount = 100, DueDate = DateTime.Now.AddDays(5).ToUniversalTime(), IsPaid = true, Type = BillType.water }
            );

            context.SaveChanges();
        }
    }
}