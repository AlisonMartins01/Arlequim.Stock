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
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _db;
        public StockRepository(AppDbContext db) => _db = db;

        public Task<bool> ProductExistsAsync(Guid productId, CancellationToken ct = default)
            => _db.Products.AnyAsync(p => p.Id == productId, ct);

        public async Task AddEntryAsync(Guid productId, int quantity, string invoiceNumber, CancellationToken ct = default)
        {
            await _db.StockEntries.AddAsync(new StockEntry
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Quantity = quantity,
                InvoiceNumber = invoiceNumber,
                CreatedAt = DateTime.UtcNow
            }, ct);
        }

        public Task<int> SumEntriesAsync(Guid productId, CancellationToken ct = default)
            => _db.StockEntries.Where(s => s.ProductId == productId).SumAsync(s => s.Quantity, ct);

        public Task<int> SumSoldAsync(Guid productId, CancellationToken ct = default)
            => _db.OrderItems.Where(i => i.ProductId == productId).SumAsync(i => i.Quantity, ct);

        public async Task<int> GetAvailableAsync(Guid productId, CancellationToken ct = default)
        {
            var product = await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId, ct);

            return product?.CurrentStock ?? 0;
        }
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public Task<Product?> GetProductForUpdateAsync(Guid productId, CancellationToken ct = default) =>
            _db.Products.FirstOrDefaultAsync(p => p.Id == productId, ct);
    }
}
