using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Entities
{
    public class StockEntry
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Product? Product { get; set; }

    }
}
