using app.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Service.Services
{
    public interface IProductService : IDisposable
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(string productSlug);
        Task<bool> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);

        List<ColorOption> GetProductColors();
        int GetColorId(string colorName);
        string GetColorName(int colorId);
    }
}
