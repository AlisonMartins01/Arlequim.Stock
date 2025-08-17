using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;

namespace Arlequim.Stock.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken ct = default);
        Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Order>> ListAsync(CancellationToken ct = default);
        Task<List<Order>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
