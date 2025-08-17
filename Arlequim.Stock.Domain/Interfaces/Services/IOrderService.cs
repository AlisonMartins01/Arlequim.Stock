using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Requests.Order;
using Arlequim.Stock.Domain.Responses;

namespace Arlequim.Stock.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderCreated> CreateAsync(Guid userId, string customerDocument, string sellerName, IEnumerable<CreateOrderProductsRequest> items, CancellationToken ct = default);
        Task<IReadOnlyList<OrderListItemResponse>> ListMineAsync(Guid userId, CancellationToken ct = default);

    }
}
