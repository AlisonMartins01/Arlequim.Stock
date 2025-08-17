using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Application.Interfaces
{
    public interface ITransaction : IAsyncDisposable
    {
        Task CommitAsync(CancellationToken ct = default);
    }
}
