using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CurrentStock { get; set; }

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public ICollection<StockEntry> StockEntries { get; set; } = new List<StockEntry>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
