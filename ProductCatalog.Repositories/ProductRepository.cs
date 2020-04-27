using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalog.Common.Models;
using ProductCatalog.Models;
using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly ProductContext _productDbContext;
        private const int defaultPageIndex = 1;
        private const int defaultPageSize = 5;
        public ProductRepository(ProductContext dbContext, ILogger<ProductRepository> logger)
        {
            _productDbContext = dbContext;
            _logger = logger;
        }

        public async Task<PaginatedList<Product>> GetProducts(int? pageNumber, int? pageSize)
        {
            try
            {
                IQueryable<Product> products = from p in _productDbContext.Products
                                               select p;

                return await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? defaultPageIndex, pageSize ?? defaultPageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetProducts().", ex);
                throw ex;
            }
        }

        public async Task<Product> GetProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    return null;
                }

                return await _productDbContext.Products.FirstOrDefaultAsync(query => query.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetProduct().", ex);
                throw ex;
            }
        }

        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                _productDbContext.Products.Add(product);
                await _productDbContext.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in CreateProduct().", ex);
                throw ex;
            }
        }
        public async Task<Product> UpdateProduct(Product product)
        {
            try
            {
                if (product?.Id == null)
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
            catch (Exception ex)
            {
                _logger.LogError("Server Error in UpdateProduct().", ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }
                var product = await _productDbContext.Products.FindAsync(id);
                _productDbContext.Products.Remove(product);
                await _productDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in DeleteProduct().", ex);
                throw ex;
            }
        }

        public async Task<List<Product>> GetProductsByIds(List<int> ids)
        {
            try
            {
                var products = await _productDbContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetProductsByIds().", ex);
                throw ex;
            }
        }
    }
}
