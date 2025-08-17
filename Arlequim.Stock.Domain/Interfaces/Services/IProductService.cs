using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Requests.Product;
using Arlequim.Stock.Domain.Responses;

namespace Arlequim.Stock.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<Guid> CreateAsync(ProductCreateAsyncRequest request, CancellationToken ct = default);
        Task UpdateAsync(ProductUpdateAsyncRequest request, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<ProductDto?> GetAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<ProductDto>> ListAsync(CancellationToken ct = default);
    }
}
