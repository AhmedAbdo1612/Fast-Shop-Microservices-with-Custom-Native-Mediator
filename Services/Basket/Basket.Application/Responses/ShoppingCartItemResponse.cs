namespace Basket.Application.Responses;

public record ShoppingCartItemResponse(
    int Quantity,
    string ImageFile,
    decimal Price,
    string ProductId,
    string ProductName
);