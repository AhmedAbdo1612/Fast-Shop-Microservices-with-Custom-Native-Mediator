using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Application.Repository;
using Discount.Core.Entities;
using MediatR;

namespace Discount.Application.Handlers;

public class UpdateDiscountHandler(IDiscountRepository discountRepository)
    : IRequestHandler<UpdateDiscountCommand, CouponDto>
{
    public async Task<CouponDto> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            Id = request.Id, Description = request.Description, Amount = request.Amount,
            ProductName = request.ProductName
        };
        var result = await discountRepository.UpdateDiscount(coupon, cancellationToken);
        return result
            ? coupon.ToCouponDto()
            : throw GrpcErrorHelper.CreateValidationException("This operation could not be performed");
    }
}