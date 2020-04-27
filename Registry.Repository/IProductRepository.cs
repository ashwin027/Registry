using Registry.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(int? pageIndex, int? pageSize);
        Task<List<Product>> GetProductsByIds(List<int> productsIds);
    }
}
