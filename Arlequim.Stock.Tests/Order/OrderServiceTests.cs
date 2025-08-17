using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;

namespace Arlequim.Stock.Tests.Order
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repo = new();
        private readonly OrderService _sut;

        public OrderServiceTests() => _sut = new OrderService(_repo.Object);

        [Fact]
        public async Task GetByUser_ReturnsOnlyUserOrders_SortedDesc()
        {
            var userId = Guid.NewGuid();
            var otherId = Guid.NewGuid();

            var data = new List<Order>{
                new(userId, 100, new DateTime(2025,8,1)),
                new(userId, 200, new DateTime(2025,8,10)),
                new(otherId, 999, new DateTime(2025,8,12))
            };

            _repo.Setup(r => r.QueryByUserAsync(userId, 1, 10, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((data.Where(x => x.UserId == userId)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Take(10)
                                    .ToList(), 2 /*total*/));

            var (items, total) = await _sut.GetByUserAsync(userId, 1, 10, CancellationToken.None);

            total.Should().Be(2);
            items.Should().HaveCount(2);
            items.Select(i => i.Amount).Should().ContainInOrder(200, 100); // desc
        }

        [Fact]
        public async Task GetByUser_Empty_WhenNoOrders()
        {
            var userId = Guid.NewGuid();
            _repo.Setup(r => r.QueryByUserAsync(userId, 3, 10, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((new List<Order>(), 0));

            var (items, total) = await _sut.GetByUserAsync(userId, 3, 10, CancellationToken.None);

            total.Should().Be(0);
            items.Should().BeEmpty();
        }
    }

    // ---- mínimos para compilar o teste ----
    public record Order(Guid UserId, decimal Amount, DateTime CreatedAt);

    public interface IOrderRepository
    {
        Task<(IReadOnlyList<Order> Items, int Total)> QueryByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct);
    }

    public class OrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) => _repo = repo;

        public Task<(IReadOnlyList<Order> Items, int Total)> GetByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct)
            => _repo.QueryByUserAsync(userId, page, pageSize, ct);
    }
}

