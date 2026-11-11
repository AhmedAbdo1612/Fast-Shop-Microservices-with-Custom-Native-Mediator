using Discount.Application.Repository;
using MediatR;

namespace Discount.Application.Commands;

public class DeleteDiscoutnHandler(IDiscountRepository discountRepository)
    : IRequestHandler<DeleteDiscountCommand, bool>
{
    public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var deleted = await discountRepository.DeleteDiscount(request.ProductName, cancellationToken);
        return deleted;
    }
}