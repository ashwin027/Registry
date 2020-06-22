using Grpc.Core;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductRepository> _logger;
        private readonly ProductClient _productClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public ProductRepository(ProductClient productClient, ILogger<ProductRepository> logger, AuthenticationStateProvider AuthenticationStateProvider)
        {
            _productClient = productClient;
            _logger = logger;
            _authenticationStateProvider = AuthenticationStateProvider;
        }
        public async Task<PagedResult<Models.Product>> GetAllProducts(int? pageIndex, int? pageSize)
        {
            try
            {
                var headers = await GetHeaders();
                var productModels = new PagedResult<Models.Product>();
                var products = await _productClient.GetProductsAsync(new ProductsRequest() { PageIndex = pageIndex, PageSize = pageSize }, headers);
                productModels.TotalPages = products.TotalPages;
                productModels.PageIndex = products.PageIndex;
                productModels.TotalCount = products.TotalCount;
                productModels.Data.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetAllProducts().", ex);
                throw ex;
            }
        }

        public async Task<List<Models.Product>> GetProductsByIds(List<int> productsIds)
        {
            try
            {
                var headers = await GetHeaders();
                var productModels = new List<Models.Product>();
                var request = new ProductByIdsRequest();
                request.Ids.AddRange(productsIds);
                var products = await _productClient.GetProductsByIdsAsync(request, headers);
                productModels.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetProductsByIds().", ex);
                throw ex;
            }
        }

        public async Task<PagedResult<Models.Product>> SearchProducts(string searchText, int? pageIndex, int? pageSize)
        {
            try
            {
                var headers = await GetHeaders();
                var productModels = new PagedResult<Models.Product>();
                var products = await _productClient.SearchProductsAsync(new SearchProductRequest() { SearchText = searchText, PageIndex = pageIndex, PageSize = pageSize }, headers);
                productModels.TotalPages = products.TotalPages;
                productModels.PageIndex = products.PageIndex;
                productModels.TotalCount = products.TotalCount;
                productModels.Data.AddRange(products.Products_.Select(p => p.ToModel()));

                return productModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in SearchProducts().", ex);
                throw ex;
            }
        }

        private async Task<Metadata> GetHeaders()
        {
            var headers = new Metadata();
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var accessToken = authState.User.Claims.FirstOrDefault(c => c.Type.Equals("access_token"));
            if (accessToken != null && !string.IsNullOrWhiteSpace(accessToken?.Value))
            {
                headers.Add("Authorization", $"Bearer {accessToken?.Value}");
            }

            return headers;
        }

    }
}
