using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ProductCatalog.Grpc.Extensions;
using ProductCatalog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Grpc.Services
{
    public class ProductService: Product.ProductBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _repository;
        public ProductService(ILogger<ProductService> logger, IProductRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogInformation("Product service initialized.");
        }
        public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                Status status;
                if (request?.ProductId == null)
                {
                    status = new Status(StatusCode.FailedPrecondition, "Product ID is null.");
                    _logger.LogError("Bad request in ProductService, method: GetProduct(). Request is null.");
                }
                else
                {
                    var product = await _repository.GetProduct(request.ProductId);

                    if (product == null)
                    {
                        status = new Status(StatusCode.NotFound, $"Product with id {request.ProductId} not found.");
                        _logger.LogError($"ProductService, method: GetProduct(). Product with id {request.ProductId} not found.");
                    }
                    else
                    {
                        return product.ToGrpcModel();
                    }
                }

                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server Error in ProductService,{context.Method}");
                throw new RpcException(Status.DefaultCancelled, ex.Message);
            }
        }

        public override async Task<Products> GetProducts(ProductsRequest request, ServerCallContext context)
        {
            Status status;
            try
            {
                var products = await _repository.GetProducts(request.PageIndex, request.PageSize);

                if (products == null)
                {
                    status = new Status(StatusCode.NotFound, $"Products not found.");
                    _logger.LogError($"ProductService, method: GetProducts(). Products not found.");
                }
                else
                {
                    var productsResponse = new Products();
                    productsResponse.Products_.Add(products.Select(p => p.ToGrpcModel()));
                    
                    return productsResponse;
                }

                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server Error in ProductService,{context.Method}");
                throw new RpcException(Status.DefaultCancelled, ex.Message);
            }
        }

        public override async Task<Products> GetProductsByIds(ProductByIdsRequest request, ServerCallContext context)
        {
            Status status;
            try
            {
                if (request?.Ids==null)
                {
                    status = new Status(StatusCode.NotFound, $"No product Ids in request.");
                    _logger.LogError($"ProductService, method: GetProducts(). No product Ids in request.");
                }
                else
                {
                    var products = await _repository.GetProductsByIds(request.Ids.ToList());

                    if (products == null)
                    {
                        status = new Status(StatusCode.NotFound, $"Products not found.");
                        _logger.LogError($"ProductService, method: GetProducts(). Products not found.");
                    }
                    else
                    {
                        var productsResponse = new Products();
                        productsResponse.Products_.Add(products.Select(p => p.ToGrpcModel()));

                        return productsResponse;
                    }
                }

                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server Error in ProductService,{context.Method}");
                throw new RpcException(Status.DefaultCancelled, ex.Message);
            }
        }
    }
}
