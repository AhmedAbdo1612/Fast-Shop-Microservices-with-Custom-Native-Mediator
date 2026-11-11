using Discount.Core.Entities;

namespace Discount.Application.Repository;

public interface IDiscountRepository
{
    Task<Coupon> GetCoupon(string productName, CancellationToken ct = default);
    Task<bool> CreateDiscount(Coupon coupon, CancellationToken ct = default);
    Task<bool> UpdateDiscount(Coupon coupon, CancellationToken ct = default);
    Task<bool> DeleteDiscount(string productName, CancellationToken ct = default);
}