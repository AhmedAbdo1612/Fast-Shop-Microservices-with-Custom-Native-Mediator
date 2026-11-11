using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Application.Repository;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<GetDiscountQuery, CouponDto>
{
    public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await discountRepository.GetCoupon(request.ProductName);
        if (coupon == null) throw GrpcErrorHelper.CreateValidationException($"Coupon {request.ProductName} not found");
        return coupon.ToCouponDto();
    }
}