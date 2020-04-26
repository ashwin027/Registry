﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductCatalog.Models.Entities;
using ProductCatalog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController: ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _repository;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _repository = productRepository;
        }

        [HttpGet]
        public ActionResult<Product> GetProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    _logger.LogError("Bad request in ProductController, method: GetProduct(). Request is null.");
                    return BadRequest();
                }
                var product = _repository.GetProduct(id);

                if (product == null)
                {
                    _logger.LogError($"ProductController, method: GetProduct(). Product with id {id} not found.");
                    return NotFound();
                }

                return Ok(product);

            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ProductController, method: GetProduct().", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            try
            {
                var products = await _repository.GetProducts();

                if (products == null)
                {
                    _logger.LogError($"ProductController, method: GetProducts(). No products found.");
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ProductController, method: GetProducts().", ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("Bad request in ProductController, method: CreateProduct(). Request is null.");
                    return BadRequest();
                }

                var result = await _repository.CreateProduct(product);

                if (result == null)
                {
                    _logger.LogError("Server Error in ProductController, method: CreateProduct(). Product creation failed.");
                    return StatusCode(500);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ProductController, method: CreateProduct().", ex);
                return StatusCode(500);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("Bad request in ProductController, method: UpdateProduct(). Request is null.");
                    return BadRequest();
                }

                var existingProduct = await _repository.GetProduct(product.Id);
                if (existingProduct == null)
                {
                    _logger.LogError("Server Error in ProductController, method: UpdateProduct(). Product not found.");
                    return NotFound();
                }

                var result = await _repository.UpdateProduct(product);

                if (result == null)
                {
                    _logger.LogError("Server Error in ProductController, method: UpdateProduct(). Product update failed.");
                    return StatusCode(500);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ProductController, method: UpdateProduct().", ex);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("Bad request in ProductController, method: DeleteProduct(). Request is null.");
                    return BadRequest();
                }

                var result  = await _repository.DeleteProduct(product.Id);

                if (!result)
                {
                    _logger.LogError("Server Error in ProductController, method: DeleteProduct(). Deletion failed.");
                    return StatusCode(500);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ProductController, method: DeleteProduct().", ex);
                return StatusCode(500);
            }
        }
    }
}
