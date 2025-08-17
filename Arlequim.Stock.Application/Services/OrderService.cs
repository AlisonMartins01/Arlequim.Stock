using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Application.Interfaces;
using Arlequim.Stock.Domain.Entities;
using Arlequim.Stock.Domain.Interfaces.Repositories;
using Arlequim.Stock.Domain.Interfaces.Services;
using Arlequim.Stock.Domain.Requests.Order;
using Arlequim.Stock.Domain.Responses;

namespace Arlequim.Stock.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork uow) : IOrderService
    {
        public async Task<OrderCreated> CreateAsync(Guid userId, string customerDocument, string sellerName, IEnumerable<CreateOrderProductsRequest> items, CancellationToken ct = default)
        {
            var list = (items ?? Enumerable.Empty<CreateOrderProductsRequest>()).ToList();
            if (!list.Any()) throw new ArgumentException("Order must have at least one item");
            if (string.IsNullOrWhiteSpace(customerDocument)) throw new ArgumentException("Customer document required");
            if (string.IsNullOrWhiteSpace(sellerName)) throw new ArgumentException("Seller name required");
            if (list.Any(i => i.Quantity <= 0)) throw new ArgumentException("All quantities must be > 0");

            var ids = list.Select(i => i.ProductId).Distinct().ToList();
            var productsDict = await productRepository.GetByIdsAsync(ids, ct);

            if (productsDict.Count != ids.Count) throw new KeyNotFoundException("Some product not found");

            // validação de estoque (sem ainda alterar)
            foreach (var it in list)
            {
                var p = productsDict[it.ProductId];
                if (p.CurrentStock < it.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product {p.Id}. Available={p.CurrentStock}, Requested={it.Quantity}");
            }

            await using (await uow.BeginTransactionAsync(ct))
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerDocument = customerDocument.Trim(),
                    SellerName = sellerName.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId
                };

                foreach (var it in list)
                {
                    var prod = productsDict[it.ProductId];

                    prod.CurrentStock -= it.Quantity;
                    productRepository.Update(prod);

                    order.Items.Add(new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = prod.Id,
                        Quantity = it.Quantity,
                        UnitPrice = prod.Price
                    });
                }

                await orderRepository.AddAsync(order, ct);
                await uow.CommitAsync(ct);          
                await uow.CommitTransactionAsync(ct);

                var total = order.Items.Sum(i => i.UnitPrice * i.Quantity);
                return new OrderCreated(order.Id, total, order.CreatedAt);
            }
        }
        public async Task<IReadOnlyList<OrderListItemResponse>> ListMineAsync(Guid userId, CancellationToken ct = default)
        {
            var orders = await orderRepository.GetByUserAsync(userId, ct);

            return orders.Select(o => new OrderListItemResponse(
                o.Id,
                o.CreatedAt,
                o.CustomerDocument,
                o.SellerName,
                o.Items.Count,
                o.Items.Sum(i => i.UnitPrice * i.Quantity)
            )).ToList();
        }
    }
}
