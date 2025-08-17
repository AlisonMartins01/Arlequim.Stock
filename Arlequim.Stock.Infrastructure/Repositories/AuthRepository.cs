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
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _db;

        public AuthRepository(AppDbContext db) => _db = db;

        public async Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var norm = Normalize(email);
            return await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == norm, ct);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            var norm = Normalize(email);
            return _db.Users.AnyAsync(u => u.Email.ToLower() == norm, ct);
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            user.Email = Normalize(user.Email);
            await _db.Users.AddAsync(user, ct);
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        private static string Normalize(string email)
            => (email ?? string.Empty).Trim().ToLowerInvariant();
    }
}
