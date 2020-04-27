using ProductCatalog.Common.Models;
using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public interface IProductRepository
    {
        Task<PaginatedList<Product>> GetProducts(int? pageNumber, int? pageSize);
        Task<PaginatedList<Product>> SearchProducts(string searchText, int? pageNumber, int? pageSize);
        Task<Product> GetProduct(int? id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int? id);
        Task<List<Product>> GetProductsByIds(List<int> ids);
    }
}
