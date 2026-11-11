using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService;

public class DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient client)
{
    public async Task<CouponModel> GetCoupon(string productName)
    {
        var request = new GetDiscountRequest { ProductName = productName };
        return await client.GetDiscountAsync(request);
    }
}