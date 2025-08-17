using System.Security.Claims;
using Arlequim.Stock.Application.Services;
using Arlequim.Stock.Domain.Interfaces.Services;
using Arlequim.Stock.Domain.Requests.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arlequim.Stock.Api.Controllers
{
    [ApiController]
    [Route("orders")]
    [Authorize(Roles = "Seller,Admin")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest req, CancellationToken ct)
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(sub)) 
                return Unauthorized();

            var userId = Guid.Parse(sub);

            var created = await orderService.CreateAsync(userId, req.CustomerDocument, req.SellerName, req.Items, ct);

            return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id) => Ok(new { id });

        [HttpGet("my-orders")]
        public async Task<IActionResult> ListMine(CancellationToken ct)
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(sub)) 
                return Unauthorized();

            var userId = Guid.Parse(sub);
            var list = await orderService.ListMineAsync(userId, ct);
            return Ok(list);
        }
    }
}
