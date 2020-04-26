using ProductCatalog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(int pageNumber=1, int pageSize=5);
        Task<Product> GetProduct(int? id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
