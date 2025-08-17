using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Requests.Order
{
    public record CreateOrderProductsRequest(Guid ProductId, int Quantity);
    public record CreateOrderRequest(string CustomerDocument, string SellerName, IEnumerable<CreateOrderProductsRequest> Items);
}
