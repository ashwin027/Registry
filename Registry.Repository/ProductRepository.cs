using ProductCatalog.Grpc;
using Registry.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProductCatalog.Grpc.Product;

namespace Registry.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductClient _productClient;
        public ProductRepository(ProductClient productClient)
        {
            _productClient = productClient;
        }
        public async Task<List<Models.Product>> GetAllProducts(int? pageIndex, int? pageSize)
        {
            try
            {
                var productModels = new List<Models.Product>();
                var products = await _productClient.GetProductsAsync(new ProductsRequest() { PageIndex = pageIndex, PageSize = pageSize });
                productModels.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<Models.Product>> GetProductsByIds(List<int> productsIds)
        {
            throw new NotImplementedException();
        }
    }
}
