using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Requests.Stock
{
    public record StockAddRequest(Guid ProductId, int Quantity, string InvoiceNumber);
}
