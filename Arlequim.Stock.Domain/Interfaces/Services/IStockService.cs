using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Requests.Stock;

namespace Arlequim.Stock.Domain.Interfaces.Services
{
    public interface IStockService
    {
        Task AddStockAsync(StockAddRequest request, CancellationToken ct = default);
        Task<int> GetAvailableAsync(Guid productId, CancellationToken ct = default);
    }
}
