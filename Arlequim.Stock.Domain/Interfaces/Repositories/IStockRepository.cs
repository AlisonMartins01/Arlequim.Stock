using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;

namespace Arlequim.Stock.Domain.Interfaces.Repositories
{
    public interface IStockRepository
    {
        Task<bool> ProductExistsAsync(Guid productId, CancellationToken ct = default);
        Task AddEntryAsync(Guid productId, int quantity, string invoiceNumber, CancellationToken ct = default);
        Task<int> SumEntriesAsync(Guid productId, CancellationToken ct = default);
        Task<int> SumSoldAsync(Guid productId, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<int> GetAvailableAsync(Guid productId, CancellationToken ct = default);

        Task<Product?> GetProductForUpdateAsync(Guid productId, CancellationToken ct = default); // inclui RowVersion

    }
}
