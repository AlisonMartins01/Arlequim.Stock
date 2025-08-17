using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Arlequim.Stock.Tests.Order
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task GetByUser_ReturnsOk_WithItems_And_TotalHeader()
        {
            // arrange
            var userId = Guid.NewGuid();
            var svc = new Mock<IOrderService>();
            svc.Setup(s => s.GetByUserAsync(userId, 1, 10, It.IsAny<CancellationToken>()))
               .ReturnsAsync((
                   new List<OrderDto> {
               new() { Id = 1, Amount = 100, CreatedAt = new DateTime(2025, 8, 10) },
               new() { Id = 2, Amount = 200, CreatedAt = new DateTime(2025, 8, 12) },
                   }.AsReadOnly(),
                   2
               ));

            var ctrl = new OrdersController(svc.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            // act
            var res = await ctrl.GetByUser(userId, 1, 10, CancellationToken.None);

            // assert
            var ok = Assert.IsType<OkObjectResult>(res);

            var items = Assert.IsAssignableFrom<IReadOnlyList<OrderDto>>(ok.Value);
            items.Should().HaveCount(2);
            items.Select(i => i.Id).Should().ContainInOrder(1, 2); // opcional

            ctrl.Response.Headers.ContainsKey("X-Total-Count").Should().BeTrue();
            ctrl.Response.Headers["X-Total-Count"].ToString().Should().Be("2");
        }

        [Theory]
        [InlineData(0, 10, 1, 10)]   
        [InlineData(1, 0, 1, 50)]    
        [InlineData(1, 999, 1, 100)] 
        public async Task GetByUser_ClampsPagination(int page, int size, int expPage, int expSize)
        {
            var userId = Guid.NewGuid();
            var svc = new Mock<IOrderService>();
            svc.Setup(s => s.GetByUserAsync(userId, expPage, expSize, It.IsAny<CancellationToken>()))
               .ReturnsAsync((Array.Empty<OrderDto>(), 0));

            var ctrl = new OrdersController(svc.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            await ctrl.GetByUser(userId, page, size, CancellationToken.None);

            svc.Verify(s => s.GetByUserAsync(userId, expPage, expSize, It.IsAny<CancellationToken>()), Times.Once);
        }

        // mínimos
        public record OrderDto { public long Id { get; set; } public decimal Amount { get; set; } public DateTime CreatedAt { get; set; } }
        public interface IOrderService { Task<(IReadOnlyList<OrderDto> Items, int Total)> GetByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct); }
        public class OrdersController : ControllerBase
        {
            private readonly IOrderService _svc;
            public OrdersController(IOrderService s) => _svc = s;

            [HttpGet("users/{userId:guid}/orders")]
            public async Task<IActionResult> GetByUser(Guid userId, int page = 1, int pageSize = 50, CancellationToken ct = default)
            {
                page = page < 1 ? 1 : page;
                pageSize = pageSize < 1 ? 50 : Math.Min(pageSize, 100);

                var (items, total) = await _svc.GetByUserAsync(userId, page, pageSize, ct);
                Response.Headers["X-Total-Count"] = total.ToString();
                return Ok(items);
            }
        }
    }
}
