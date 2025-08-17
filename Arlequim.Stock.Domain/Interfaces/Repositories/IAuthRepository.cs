using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;

namespace Arlequim.Stock.Domain.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
