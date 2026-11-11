namespace Basket.Application.DTOs;

public record ShoppingCartDto(
    string UserName,
    List<ShoppingCartItemDto> Items
)
{
    public decimal TotalPrice => Items.Sum(x => x.Price);
}

public record ShoppingCartItemDto(
    string ProductId,
    string ProductName,
    string ImageFile,
    decimal Price,
    int Quantity
);
public record BasketChecoutDto(
    string UserName,
    decimal TotalPrice,
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode,
    string CardName,
    string CardNumber,
    string Cvv,
    string Expiration,
    int PaymentMethod
    );