using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Application.Repository;
using Discount.Core.Entities;
using MediatR;

namespace Discount.Application.Handlers;

public class CreateDiscountHandler(IDiscountRepository discountRepository)
    : IRequestHandler<CreateDiscountCommand, CouponDto>
{
    public async Task<CouponDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            ProductName = request.ProductName,
            Description = request.Description,
            Amount = request.Amount
        };
        var success = await discountRepository.CreateDiscount(coupon, cancellationToken);
        if (!success)
            throw GrpcErrorHelper.CreateValidationException(
                "There are some server errors right new please try again later");
        return coupon.ToCouponDto();
    }
}