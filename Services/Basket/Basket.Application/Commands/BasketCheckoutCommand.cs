using Basket.Application.DTOs;
using MediatR;

namespace Basket.Application.Commands;

public record BasketCheckoutCommand(BasketChecoutDto BasketChecoutDto) :IRequest<Unit>;