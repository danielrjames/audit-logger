using app.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Data.Repositories
{
    public interface IProductRepository : IDisposable
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string productSlug);
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
    }
}
