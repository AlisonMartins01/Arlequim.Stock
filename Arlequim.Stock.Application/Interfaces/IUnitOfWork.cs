using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken ct = default);
        Task<IAsyncDisposable> BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
    }
}
