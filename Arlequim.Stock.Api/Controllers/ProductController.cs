using Arlequim.Stock.Domain.Interfaces.Services;
using Arlequim.Stock.Domain.Requests.Product;
using Arlequim.Stock.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arlequim.Stock.Api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductCreateAsyncRequest request, CancellationToken ct)
        {
            var id = await productService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // --- UPDATE (Admin) ---
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateAsyncRequest request, CancellationToken ct)
        {
            await productService.UpdateAsync(request, ct);
            return NoContent();
        }

        // --- DELETE (Admin) ---
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await productService.DeleteAsync(id, ct);
            return NoContent();
        }

        // --- GET BY ID (Public/Authenticated, ajuste se quiser restringir) ---
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto?>> GetById(Guid id, CancellationToken ct)
        {
            var product = await productService.GetAsync(id, ct);
            if (product is null) return NotFound();
            return Ok(product);
        }

        // --- LIST ALL (Public/Authenticated) ---
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> List(CancellationToken ct)
        {
            var items = await productService.ListAsync(ct);
            return Ok(items);
        }
    }
}
