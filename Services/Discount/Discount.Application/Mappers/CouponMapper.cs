using Discount.Application.DTOs;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mappers;

public static class CouponMapper
{
    public static CouponDto ToCouponDto(this Coupon c) => new CouponDto(c.Id, c.ProductName, c.Description, c.Amount);

    public static CouponModel ToCouponModel(this CouponDto dto) => new CouponModel
    {
        Id = dto.Id,
        ProductName = dto.ProductName,
        Description = dto.Description,
        Amount = dto.Amount,
    };
   
}