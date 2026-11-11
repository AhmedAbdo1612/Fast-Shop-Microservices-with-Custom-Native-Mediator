using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderDbSeed
{
    public static async Task Seed(OrderDbContext context, ILogger<OrderDbSeed> logger)
    {
        if (!context.Orders.Any())
        {
            context.Orders.AddRange(GetOrders());
            await context.SaveChangesAsync();
            logger.LogInformation("Orders have been created.");
        }
    }

    public static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
        {
            new Order
            {
                UserName = "ahmed.ali",
                TotalPrice = 250.00m,
                FirstName = "Ahmed",
                LastName = "Ali",
                EmailAddress = "ahmed.ali@example.com",
                AddressLine = "123 Tahrir Street",
                Country = "Egypt",
                State = "Cairo",
                ZipCode = "11511",
                CardName = "Ahmed Ali",
                CardNumber = "**** **** **** 1234",
                Cvv = "123",
                Expiration = "12/28",
                PaymentMethod = 1
            },
            new Order
            {
                UserName = "sara.mohamed",
                TotalPrice = 450.50m,
                FirstName = "Sara",
                LastName = "Mohamed",
                EmailAddress = "sara.m@example.com",
                AddressLine = "45 Nile Corniche",
                Country = "Egypt",
                State = "Giza",
                ZipCode = "12511",
                CardName = "Sara Mohamed",
                CardNumber = "**** **** **** 5678",
                Cvv = "456",
                Expiration = "08/27",
                PaymentMethod = 1
            },
            new Order
            {
                UserName = "john.doe",
                TotalPrice = 120.00m,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                AddressLine = "789 Broadway Ave",
                Country = "USA",
                State = "New York",
                ZipCode = "10001",
                CardName = "John Doe",
                CardNumber = "**** **** **** 9876",
                Cvv = "789",
                Expiration = "05/29",
                PaymentMethod = 2
            },
            new Order
            {
                UserName = "fatima.hassan",
                TotalPrice = 890.00m,
                FirstName = "Fatima",
                LastName = "Hassan",
                EmailAddress = "fatima.h@example.com",
                AddressLine = "12 King Fahd Road",
                Country = "Saudi Arabia",
                State = "Riyadh",
                ZipCode = "11564",
                CardName = "Fatima Hassan",
                CardNumber = "**** **** **** 4321",
                Cvv = "321",
                Expiration = "10/26",
                PaymentMethod = 1
            },
            new Order
            {
                UserName = "omar.farouk",
                TotalPrice = 65.25m,
                FirstName = "Omar",
                LastName = "Farouk",
                EmailAddress = "omar.f@example.com",
                AddressLine = "56 Al-Horreya St",
                Country = "Egypt",
                State = "Alexandria",
                ZipCode = "21521",
                CardName = "Omar Farouk",
                CardNumber = "**** **** **** 8765",
                Cvv = "654",
                Expiration = "03/30",
                PaymentMethod = 3
            },
            new Order
            {
                UserName = "emily.smith",
                TotalPrice = 310.00m,
                FirstName = "Emily",
                LastName = "Smith",
                EmailAddress = "emily.smith@example.com",
                AddressLine = "32 Oxford Street",
                Country = "UK",
                State = "London",
                ZipCode = "W1D 1BS",
                CardName = "Emily Smith",
                CardNumber = "**** **** **** 1122",
                Cvv = "111",
                Expiration = "07/28",
                PaymentMethod = 2
            },
            new Order
            {
                UserName = "mohamed.ibrahim",
                TotalPrice = 1250.75m,
                FirstName = "Mohamed",
                LastName = "Ibrahim",
                EmailAddress = "mohamed.i@example.com",
                AddressLine = "99 Zayed City",
                Country = "Egypt",
                State = "Giza",
                ZipCode = "12588",
                CardName = "Mohamed Ibrahim",
                CardNumber = "**** **** **** 3344",
                Cvv = "222",
                Expiration = "09/27",
                PaymentMethod = 1
            },
            new Order
            {
                UserName = "anna.wagner",
                TotalPrice = 199.99m,
                FirstName = "Anna",
                LastName = "Wagner",
                EmailAddress = "anna.w@example.com",
                AddressLine = "14 Berliner Strasse",
                Country = "Germany",
                State = "Berlin",
                ZipCode = "10115",
                CardName = "Anna Wagner",
                CardNumber = "**** **** **** 5566",
                Cvv = "333",
                Expiration = "11/29",
                PaymentMethod = 2
            },
            new Order
            {
                UserName = "khaled.mahmoud",
                TotalPrice = 540.00m,
                FirstName = "Khaled",
                LastName = "Mahmoud",
                EmailAddress = "khaled.m@example.com",
                AddressLine = "88 Al-Malik Abdulaziz",
                Country = "Saudi Arabia",
                State = "Jeddah",
                ZipCode = "21442",
                CardName = "Khaled Mahmoud",
                CardNumber = "**** **** **** 7788",
                Cvv = "444",
                Expiration = "02/26",
                PaymentMethod = 1
            },
            new Order
            {
                UserName = "lucas.silva",
                TotalPrice = 780.50m,
                FirstName = "Lucas",
                LastName = "Silva",
                EmailAddress = "lucas.s@example.com",
                AddressLine = "500 Avenida Paulista",
                Country = "Brazil",
                State = "São Paulo",
                ZipCode = "01310-100",
                CardName = "Lucas Silva",
                CardNumber = "**** **** **** 9900",
                Cvv = "555",
                Expiration = "06/30",
                PaymentMethod = 3
            }
        };
    }
}