using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arlequim.Stock.Infrastructure.Persistence
{
    public class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!await db.Users.AnyAsync())
            {
                var hasher = new Security.BcryptPasswordHasher();
                db.Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@local",
                    PasswordHash = hasher.Hash("Admin@123"),
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
