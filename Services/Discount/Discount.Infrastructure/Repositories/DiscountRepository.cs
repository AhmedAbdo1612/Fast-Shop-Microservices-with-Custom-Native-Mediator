using System.Data;
using Dapper;
using Discount.Application.Repository;
using Discount.Core.Entities;

namespace Discount.Infrstructure.Repositories;

public class DiscountRepository(IDbConnection dbConnection) : IDiscountRepository
{
    public async Task<Coupon> GetCoupon(string productName, CancellationToken ct = default)
    {
        var coupon =
            await dbConnection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM coupons WHERE Productname=@productName", new { productName });
        return coupon ?? new Coupon
        {
            ProductName = "No Discount",
            Amount = 0,
            Description = "No Discount"
        };
    }

    public async Task<bool> CreateDiscount(Coupon coupon, CancellationToken ct = default)
    {
        var afftected = await dbConnection.ExecuteAsync(
            "Insert Into Coupons (ProductName, Description, Amount) Values (@ProductName, @Description, @Amount)",
            new { coupon.ProductName, coupon.Description, coupon.Amount }
        );
        return afftected > 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon, CancellationToken ct = default)
    {
        var affected = await dbConnection.ExecuteAsync(
            "Update Coupons Set ProductName =@ProuctName, Description=@Description, Amount=@Amount Where Id=@Id",
            new {coupon.Id, coupon.ProductName, coupon.Description, coupon.Amount });
        return affected > 0;
    }

    public async Task<bool> DeleteDiscount(string productName, CancellationToken ct = default)
    {
        var affected = await dbConnection.ExecuteAsync("Delete From Coupons where ProductName = @ProductName",
            new { ProductName = productName });
        return affected > 0;
    }
}