using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllProductsQuery(catalogSpecParams), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("productName/{productName}")]
    public async Task<IActionResult> GetProductsByName(string productName, CancellationToken cancellationToken)
    {
        var query = new GetProductByNameQuery(productName);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }

    [HttpGet("GetAllBrands")]
    public async Task<IActionResult> GetAllBrands( CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllBrandsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("GetAllTypes")]
    public async Task<IActionResult> GetAllTypes( CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllTypesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("/brand/{brand}")]
    public async Task<IActionResult> GetProductsByBrand(string brand, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductsByBrandQuery(brand), ct);
        return Ok(result);
    }
}