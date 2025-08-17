using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerDocument { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
