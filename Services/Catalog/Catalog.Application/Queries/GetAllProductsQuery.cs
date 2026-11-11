using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Application.Queries;

public record GetAllProductsQuery(CatalogSpecParams CatalogSpecParams) : IRequest<Pagination<ProductResponse>>;