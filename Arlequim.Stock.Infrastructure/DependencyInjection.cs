using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Application.Interfaces;
using Arlequim.Stock.Application.Services;
using Arlequim.Stock.Domain.Interfaces.Repositories;
using Arlequim.Stock.Infrastructure.Identity;
using Arlequim.Stock.Infrastructure.Persistence;
using Arlequim.Stock.Infrastructure.Repositories;
using Arlequim.Stock.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arlequim.Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(cfg.GetConnectionString("Default")));


            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
