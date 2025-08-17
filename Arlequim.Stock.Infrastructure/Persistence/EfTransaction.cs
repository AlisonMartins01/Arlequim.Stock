using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Arlequim.Stock.Infrastructure.Persistence
{
    public sealed class EfTransaction : ITransaction
    {
        private readonly IDbContextTransaction _tx;
        private bool _committed;

        public EfTransaction(IDbContextTransaction tx) => _tx = tx;

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _tx.CommitAsync(ct);
            _committed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_committed)
                await _tx.RollbackAsync();
            await _tx.DisposeAsync();
        }
    }
}
