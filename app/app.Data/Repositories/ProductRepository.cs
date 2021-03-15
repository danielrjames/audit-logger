using app.Data.Contexts;
using app.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Data.Repositories
{
    /// <summary>
    /// Product repo for interacting with the dbcontext
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Product repo constructor
        /// </summary>
        /// <param name="context">dbcontext injection</param>
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve products from dbcontext
        /// </summary>
        /// <returns>list of products</returns>
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Get product by url slug
        /// </summary>
        /// <param name="productSlug">url slug of product</param>
        /// <returns>product</returns>
        public async Task<Product> GetProduct(string productSlug)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Slug == productSlug);
        }

        /// <summary>
        /// Add new product to dbcontext
        /// </summary>
        /// <param name="product">product to be added</param>
        /// <returns>boolean add result</returns>
        public async Task<bool> AddProduct(Product product)
        {
            if (product.Id == 0) // only add if id is 0
            {
                _context.Products.Add(product);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Update existing product
        /// </summary>
        /// <param name="product">product to be updated</param>
        /// <returns>boolean update result</returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            var dbEntry = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            if (dbEntry != null) // if entry exists, make edits
            {
                dbEntry.Name = product.Name;
                dbEntry.Color = product.Color;
                dbEntry.Price = product.Price;

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Delete existing product
        /// </summary>
        /// <param name="productId">id of product to be deleted</param>
        /// <returns>boolean delete result</returns>
        public async Task<bool> DeleteProduct(int productId)
        {
            var dbEntry = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (dbEntry != null) // if entry exists, delete
            {
                _context.Remove(dbEntry);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
