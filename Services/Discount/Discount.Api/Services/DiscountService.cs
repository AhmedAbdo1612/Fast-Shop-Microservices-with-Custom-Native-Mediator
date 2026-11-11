using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Api.Services;

public class DiscountService(IMediator mediator) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var query = new GetDiscountQuery(request.ProductName);
        var dto = await mediator.Send(query);
        return dto.ToCouponModel();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var model = request.Coupon;
        var command = new CreateDiscountCommand(model.ProductName, model.Description, model.Amount);
        var dto = await mediator.Send(command);
        return dto.ToCouponModel();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var model = request.Coupon;
        var command = new UpdateDiscountCommand(model.Id, model.ProductName, model.Description, model.Amount);
        var dto = await mediator.Send(command);
        return dto.ToCouponModel();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var command = new DeleteDiscountCommand(request.ProductName);
        var result = await mediator.Send(command);
        return new DeleteDiscountResponse { Success = result };
    }
}