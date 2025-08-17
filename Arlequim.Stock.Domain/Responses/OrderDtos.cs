using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Responses
{
    public record OrderCreated(Guid OrderId, decimal Total, DateTime CreatedAt);
    public record OrderListItemResponse(Guid Id, DateTime CreatedAt, string CustomerDocument, string SellerName, int Items, decimal Total);
}
