using Microsoft.EntityFrameworkCore;
using ProductCatalog.Common.Models;
using ProductCatalog.Models;
using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly ProductContext _productDbContext;
        public ProductRepository(ProductContext dbContext)
        {
            _productDbContext = dbContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _productDbContext.Products.Add(product);
            await _productDbContext.SaveChangesAsync();

            return product;
        }

        public async void DeleteProduct(int id)
        {
            var product = await _productDbContext.Products.FindAsync(id);
            _productDbContext.Products.Remove(product);
            await _productDbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _productDbContext.Products.FirstOrDefaultAsync(query => query.Id==id);
        }

        public async Task<List<Product>> GetProducts(int pageNumber, int pageSize)
        {
            IQueryable<Product> products = from p in _productDbContext.Products
                                             select p;

            return await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            if (product?.Id==null)
            {
                return null;
            }

            var existingProduct = await _productDbContext.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Id = product.Id;
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            await _productDbContext.SaveChangesAsync();

            return existingProduct;
        }
    }
}
