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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Product product, CancellationToken ct = default)
        {
            await _db.Products.AddAsync(product, ct);
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            // Se quiser carregar navegações, adicione Include(...)
            return _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public Task<List<Product>> ListAsync(CancellationToken ct = default)
        {
            return _db.Products
                      .OrderByDescending(p => p.CreatedAt)
                      .ToListAsync(ct);
        }

        public async Task<bool> HasStockOrOrderItemsAsync(Guid productId, CancellationToken ct = default)
        {
            var hasStock = await _db.StockEntries.AnyAsync(s => s.ProductId == productId, ct);
            if (hasStock) return true;

            var hasOrderItems = await _db.OrderItems.AnyAsync(i => i.ProductId == productId, ct);
            return hasOrderItems;
        }

        public void Remove(Product product)
        {
            _db.Products.Remove(product);
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
        public Task<Dictionary<Guid, Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
            => _db.Products
                  .Where(p => ids.Contains(p.Id))
                  .ToDictionaryAsync(p => p.Id, ct);
    }
}
