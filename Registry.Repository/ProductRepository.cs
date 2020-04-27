using ProductCatalog.Grpc;
using Registry.Models;
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
        public async Task<PagedResult<Models.Product>> GetAllProducts(int? pageIndex, int? pageSize)
        {
            try
            {
                var productModels = new PagedResult<Models.Product>();
                var products = await _productClient.GetProductsAsync(new ProductsRequest() { PageIndex = pageIndex, PageSize = pageSize });
                productModels.TotalPages = products.TotalPages;
                productModels.PageIndex = products.PageIndex;
                productModels.TotalCount = products.TotalCount;
                productModels.Data.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Models.Product>> GetProductsByIds(List<int> productsIds)
        {
            try
            {
                var productModels = new List<Models.Product>();
                var request = new ProductByIdsRequest();
                request.Ids.AddRange(productsIds);
                var products = await _productClient.GetProductsByIdsAsync(request);
                productModels.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedResult<Models.Product>> SearchProducts(string searchText, int? pageIndex, int? pageSize)
        {
            try
            {
                var productModels = new PagedResult<Models.Product>();
                var products = await _productClient.SearchProductsAsync(new SearchProductRequest() { SearchText = searchText, PageIndex = pageIndex, PageSize = pageSize });
                productModels.TotalPages = products.TotalPages;
                productModels.PageIndex = products.PageIndex;
                productModels.TotalCount = products.TotalCount;
                productModels.Data.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
