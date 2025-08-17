using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Arlequim.Stock.Application.Interfaces;
using Arlequim.Stock.Domain.Entities;
using Arlequim.Stock.Domain.Interfaces.Repositories;
using Arlequim.Stock.Domain.Interfaces.Services;
using Arlequim.Stock.Domain.Requests.Stock;

namespace Arlequim.Stock.Application.Services
{
    public class StockService(IStockRepository stockRepository, IProductRepository productRepository, IUnitOfWork unitOfWork) : IStockService
    {


        public async Task AddStockAsync(StockAddRequest request, CancellationToken ct = default)
        {
            if (request.Quantity <= 0) throw new ArgumentException("Quantity must be > 0");
            if (string.IsNullOrWhiteSpace(request.InvoiceNumber)) throw new ArgumentException("Invoice number is required");

            var product = await productRepository.GetByIdAsync(request.ProductId, ct)
                          ?? throw new KeyNotFoundException("Product not found");

            await using var scope = await unitOfWork.BeginTransactionAsync(ct);
            
            await stockRepository.AddEntryAsync(product.Id, request.Quantity, request.InvoiceNumber.Trim(), ct);

            product.CurrentStock += request.Quantity;
            productRepository.Update(product); 

            await unitOfWork.CommitAsync(ct);
            await unitOfWork.CommitTransactionAsync(ct);
        }

        public async Task<int> GetAvailableAsync(Guid productId, CancellationToken ct = default) => await stockRepository.GetAvailableAsync(productId, ct);
    }
}
