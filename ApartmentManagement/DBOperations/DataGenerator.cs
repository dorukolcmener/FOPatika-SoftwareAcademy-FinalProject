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
                new User { Name = "Touka", Surname = "Kirishima", EMail = "touka.kirishima@test.com", Password = "test123", Phone = "+1231231223", TCNo = 12345678901, Role = UserType.renter, Balance = 1000 },
                new User { Name = "Avatar", Surname = "Korra", EMail = "avatar.korra@test.com", Password = "test123", Phone = "+44231231223", TCNo = 12345678901, Role = UserType.admin, Balance = 1000 }
            );

            context.SaveChanges();

            context.Vehicles.AddRange(
                new Vehicle { LicensePlate = "34 ABC 34", OwnerId = context.Users.First().Id },
                new Vehicle { LicensePlate = "34 ABC 35", OwnerId = context.Users.First().Id },
                new Vehicle { LicensePlate = "34 ABC 36", OwnerId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id }
            );

            context.Apartments.AddRange(
                new Apartment { Block = "A Block", Floor = 1, Door = 1, UserId = context.Users.First().Id, FlatType = "1+1" },
                new Apartment { Block = "A Block", Floor = 1, Door = 2, UserId = context.Users.First().Id, FlatType = "2+1" },
                new Apartment { Block = "A Block", Floor = 1, Door = 3, UserId = context.Users.First().Id, FlatType = "3+1" },
                new Apartment { Block = "A Block", Floor = 2, Door = 4, UserId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id, FlatType = "1+1" },
                new Apartment { Block = "A Block", Floor = 2, Door = 5, UserId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id, FlatType = "2+1" },
                new Apartment { Block = "A Block", Floor = 2, Door = 6, UserId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id, FlatType = "3+1" }
            );

            context.SaveChanges();

            context.Bills.AddRange(
                new Bill { ApartmentId = context.Apartments.First().Id, Amount = 100.5, DueDate = DateTime.Now.AddDays(5).ToUniversalTime(), IsPaid = true, Type = BillType.monthly },
                new Bill { ApartmentId = context.Apartments.First().Id, Amount = 50.2, DueDate = DateTime.Now.AddDays(15).ToUniversalTime(), IsPaid = false, Type = BillType.electricity },
                new Bill { ApartmentId = context.Apartments.Where(apt => apt.Id == 4).FirstOrDefault().Id, Amount = 30.3, DueDate = DateTime.Now.AddDays(5).ToUniversalTime(), IsPaid = true, Type = BillType.water },
                new Bill { ApartmentId = context.Apartments.Where(apt => apt.Id == 5).FirstOrDefault().Id, Amount = 30.3, DueDate = DateTime.Now.AddDays(5).ToUniversalTime(), IsPaid = true, Type = BillType.monthly },
                new Bill { ApartmentId = context.Apartments.Where(apt => apt.Id == 6).FirstOrDefault().Id, Amount = 30.3, DueDate = DateTime.Now.AddDays(5).ToUniversalTime(), IsPaid = true, Type = BillType.water }
            );

            context.Messages.AddRange(
                new Message { Content = "Hello, how are you?", Date = DateTime.Now.AddDays(-5).ToUniversalTime(), FromId = context.Users.First().Id, ToId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id },
                new Message { Content = "I'm fine, thanks.", Date = DateTime.Now.AddDays(-4).ToUniversalTime(), FromId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id, ToId = context.Users.First().Id },
                new Message { Content = "What's up?", Date = DateTime.Now.AddDays(-3).ToUniversalTime(), FromId = context.Users.First().Id, ToId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id },
                new Message { Content = "Nothing much, just chilling.", Date = DateTime.Now.AddDays(-2).ToUniversalTime(), FromId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id, ToId = context.Users.First().Id },
                new Message { Content = "Cool, see you later.", Date = DateTime.Now.AddDays(-1).ToUniversalTime(), FromId = context.Users.First().Id, ToId = context.Users.Where(user => user.Id == 2).FirstOrDefault().Id }
            );

            context.SaveChanges();
        }
    }
}