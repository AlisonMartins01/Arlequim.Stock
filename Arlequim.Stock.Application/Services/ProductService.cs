using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;
using Arlequim.Stock.Domain.Interfaces.Repositories;
using Arlequim.Stock.Domain.Interfaces.Services;
using Arlequim.Stock.Domain.Requests.Product;
using Arlequim.Stock.Domain.Responses;

namespace Arlequim.Stock.Application.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public async Task<Guid> CreateAsync(ProductCreateAsyncRequest request, CancellationToken ct = default)
        {
            ValidateName(request.Name);
            ValidatePrice(request.Price);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                Price = request.Price,
                CreatedAt = DateTime.UtcNow
            };

            await productRepository.AddAsync(product, ct);
            await productRepository.SaveChangesAsync(ct);
            return product.Id;
        }

        public async Task UpdateAsync(ProductUpdateAsyncRequest request, CancellationToken ct = default)
        {
            ValidateName(request.Name);
            ValidatePrice(request.Price);

            var existing = await productRepository.GetByIdAsync(request.Id, ct)
                ?? throw new KeyNotFoundException("Product not found");

            existing.Name = request.Name.Trim();
            existing.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            existing.Price = request.Price;

            await productRepository.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await productRepository.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException("Product not found");

            if (await productRepository.HasStockOrOrderItemsAsync(id, ct))
                throw new InvalidOperationException("Cannot delete a product with stock entries or order items.");

            productRepository.Remove(existing);
            await productRepository.SaveChangesAsync(ct);
        }

        public async Task<ProductDto?> GetAsync(Guid id, CancellationToken ct = default)
        {
            var p = await productRepository.GetByIdAsync(id, ct);
            return p is null ? null : Map(p);
        }

        public async Task<IEnumerable<ProductDto>> ListAsync(CancellationToken ct = default)
        {
            var list = await productRepository.ListAsync(ct);
            return list.Select(Map).ToList();
        }

        private static ProductDto Map(Product p) =>
            new(p.Id, p.Name, p.Description, p.Price, p.CreatedAt);

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Trim().Length < 2)
                throw new ArgumentException("Name must have at least 2 characters.", nameof(name));
        }

        private static void ValidatePrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));
        }
    }
}
