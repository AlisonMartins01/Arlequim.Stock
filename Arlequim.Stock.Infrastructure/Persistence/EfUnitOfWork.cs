using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Arlequim.Stock.Infrastructure.Persistence
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _currentTx;

        public EfUnitOfWork(AppDbContext db) => _db = db;

        public Task<int> CommitAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public async Task<IAsyncDisposable> BeginTransactionAsync(CancellationToken ct = default)
        {
            _currentTx = await _db.Database.BeginTransactionAsync(ct);
            // IDbContextTransaction implementa IAsyncDisposable, então podemos retornar ele mesmo
            return _currentTx;
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTx is null)
                throw new InvalidOperationException("No active transaction to commit.");

            await _currentTx.CommitAsync(ct);
        }
    }
}
