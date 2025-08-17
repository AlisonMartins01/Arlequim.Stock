using Arlequim.Stock.Domain.Interfaces.Services;
using Arlequim.Stock.Domain.Requests.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arlequim.Stock.Api.Controllers
{
    [ApiController]
    [Route("stock")]
    public class StockController(IStockService stockService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] StockAddRequest req, CancellationToken ct)
        {
            await stockService.AddStockAsync(req, ct);
            return NoContent();
        }

        // Saldo disponível (pode deixar público para testes)
        [HttpGet("{productId:guid}/available")]
        [AllowAnonymous]
        public async Task<IActionResult> Available(Guid productId, CancellationToken ct)
        {
            var available = await stockService.GetAvailableAsync(productId, ct);
            return Ok(new { productId, available });
        }
    }
}
