using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Application.Interfaces;
using Arlequim.Stock.Domain.Entities;
using Arlequim.Stock.Domain.Interfaces.Repositories;

namespace Arlequim.Stock.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(IAuthRepository repo, IPasswordHasher hasher, IJwtTokenGenerator jwt)
        {
            _repo = repo; _hasher = hasher; _jwt = jwt;
        }

        public async Task<string> RegisterAsync(string name, string email, string password, string role, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters.", nameof(password));
            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                throw new ArgumentException("Invalid role. Use 'Admin' or 'Seller'.", nameof(role));

            if (await _repo.EmailExistsAsync(email, ct))
                throw new InvalidOperationException("Email already in use.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Email = email.Trim(),
                PasswordHash = _hasher.Hash(password),
                Role = parsedRole,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(user, ct);
            await _repo.SaveChangesAsync(ct);

            return _jwt.Create(user);
        }

        public async Task<string> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password are required.");

            var user = await _repo.FindByEmailAsync(email, ct);
            if (user is null || !_hasher.Verify(password, user.PasswordHash))
                throw new InvalidOperationException("Invalid credentials.");

            return _jwt.Create(user);
        }
    }
}
