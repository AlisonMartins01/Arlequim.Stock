using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;

namespace Arlequim.Stock.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product, CancellationToken ct = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Product>> ListAsync(CancellationToken ct = default);
        Task<bool> HasStockOrOrderItemsAsync(Guid productId, CancellationToken ct = default);
        void Remove(Product product);
        void Update(Product product);
        Task<int> SaveChangesAsync(CancellationToken ct = default); 
        Task<Dictionary<Guid, Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

    }
}
