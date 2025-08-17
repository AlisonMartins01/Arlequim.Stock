using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;
using Arlequim.Stock.Domain.Interfaces.Repositories;
using Arlequim.Stock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Arlequim.Stock.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) => _db = db;

        public Task AddAsync(Order order, CancellationToken ct = default) => _db.Orders.AddAsync(order, ct).AsTask();
        public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, ct);
        public Task<List<Order>> ListAsync(CancellationToken ct = default) =>
            _db.Orders.AsNoTracking().Include(o => o.Items).OrderByDescending(o => o.CreatedAt).ToListAsync(ct);
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
        public Task<List<Order>> GetByUserAsync(Guid userId, CancellationToken ct = default) =>
            _db.Orders.AsNoTracking()
              .Where(o => o.UserId == userId)
              .Include(o => o.Items)
              .OrderByDescending(o => o.CreatedAt)
              .ToListAsync(ct);
    }
}
