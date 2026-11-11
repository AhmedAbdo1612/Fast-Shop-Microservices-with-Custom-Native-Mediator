using Grpc.Core;
using Grpc.AspNetCore;

namespace Discount.Application.Extensions;

public static class GrpcErrorHelper
{
    public static RpcException CreateValidationException( string message)
    {
     return new RpcException(new Status(StatusCode.Internal, message));
    }
}