using app.Data.Repositories;
using app.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Service.Services
{
    /// <summary>
    /// Product service class to interact with product repo
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly string[] colors = { "red", "yellow", "blue", "green", "black", "white" }; // ideally we would save these in the db
        private readonly IProductRepository _repo;

        /// <summary>
        /// product service constructor
        /// </summary>
        /// <param name="repo">injected product repo</param>
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// calls product repo to get all products
        /// </summary>
        /// <returns>list of products</returns>
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _repo.GetProducts();
        }

        /// <summary>
        /// call product repo to get product by url slug
        /// </summary>
        /// <param name="productSlug">url slug of product</param>
        /// <returns>product</returns>
        public async Task<Product>GetProduct(string productSlug)
        {
            return await _repo.GetProduct(productSlug);
        }

        /// <summary>
        /// calls product repo to create new product
        /// </summary>
        /// <param name="product">product to be created</param>
        /// <returns>boolean create result</returns>
        public async Task<bool> CreateProduct(Product product)
        {
            if (product != null) // null check from DTO controller method
            {
                return await _repo.AddProduct(product);
            }

            return false;
        }

        /// <summary>
        /// calls product repo to update existing product
        /// </summary>
        /// <param name="product">product to be updated</param>
        /// <returns>boolean update result</returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            if (product != null) // null check from DTO controller method
            {
                return await _repo.UpdateProduct(product);
            }

            return false;
        }

        /// <summary>
        /// calls product repo to delete existing product by id
        /// </summary>
        /// <param name="productId">id of product to be deleted</param>
        /// <returns>boolean delete result</returns>
        public async Task<bool> DeleteProduct(int productId)
        {
            if (productId > 0) // id of 0 does not exist
            {
                return await _repo.DeleteProduct(productId);
            }

            return false;
        }

        /// <summary>
        /// get color options for creat/edit product color list
        /// </summary>
        /// <returns>list of color options</returns>
        public List<ColorOption> GetProductColors()
        {
            var colorList = new List<ColorOption>();

            for (int i = 0; i < colors.Length; i++)
            {
                colorList.Add(new ColorOption
                {
                    Id = i + 1,
                    Name = colors[i]
                });
            }

            return colorList;
        }

        /// <summary>
        /// get color id from list of colors
        /// </summary>
        /// <returns>color id</returns>
        public int GetColorId(string colorName)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] == colorName)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// get color name from list of colors
        /// </summary>
        /// <returns>color name</returns>
        public string GetColorName(int colorId)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if (i + 1 == colorId)
                {
                    return colors[i];
                }
            }

            return null;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _repo.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
