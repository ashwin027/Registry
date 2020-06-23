using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reviews.Models.Entities;
using Reviews.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProductCatalog.Grpc.Product;
using ProductCatalog.Grpc;
using Reviews.Models;
using Microsoft.AspNetCore.Authorization;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using IdentityModel.Client;

namespace Reviews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewRepository _repository;
        private readonly ProductClient _productClient;
        private readonly IHttpClientFactory _httpClientFactory;
        public ReviewController(ILogger<ReviewController> logger, 
            IReviewRepository reviewRepository, 
            ProductClient productClient,
            IHttpClientFactory HttpClientFactory,
            IConfiguration Configuration)
        {
            _logger = logger;
            _repository = reviewRepository;
            _productClient = productClient;
            _configuration = Configuration;
            _httpClientFactory = HttpClientFactory;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            try
            {
                var review = await _repository.GetReview(id);

                if (review == null)
                {
                    _logger.LogError($"ReviewController, method: GetReview(). Review with id {id} not found.");
                    return NotFound();
                }

                return Ok(review);

            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: GetReview().", ex);
                return StatusCode(500);
            }
        }

        [HttpGet("/api/reviews")]
        public async Task<ActionResult<PaginatedList<Review>>> GetReviews(int? pageIndex, int? pageSize)
        {
            try
            {
                var reviews = await _repository.GetReviews(pageIndex, pageSize);

                if (reviews == null)
                {
                    _logger.LogError($"ReviewController, method: GetReviews(). No reviews found.");
                    return NotFound();
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: GetReviews().", ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            try
            {
                if (review == null)
                {
                    _logger.LogError("Bad request in ReviewController, method: CreateReview(). Request is null.");
                    return BadRequest("Request is null");
                }

                var userToken = await HttpContext.GetTokenAsync(Constants.AccessTokenClaimType);
                var product = _productClient.GetProduct(new ProductRequest() { ProductId = review.ProductId }, await GetHeaders(userToken));
                if (product == null)
                {
                    _logger.LogError($"Bad request in ReviewController, method: CreateReview(). Product with id {review.ProductId} does not exist.");
                    return BadRequest($"Product with id {review.ProductId} does not exist.");
                }

                var result = await _repository.CreateReview(review);

                if (result == null)
                {
                    _logger.LogError("Server Error in ReviewController, method: CreateReview(). Review creation failed.");
                    return StatusCode(500);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: CreateReview().", ex);
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Review>> UpdateReview(Review review)
        {
            try
            {
                if (review == null)
                {
                    _logger.LogError("Bad request in ReviewController, method: UpdateReview(). Request is null.");
                    return BadRequest();
                }

                var userToken = await HttpContext.GetTokenAsync(Constants.AccessTokenClaimType);
                var product = _productClient.GetProduct(new ProductRequest() { ProductId = review.ProductId }, await GetHeaders(userToken));
                if (product == null)
                {
                    _logger.LogError($"Bad request in ReviewController, method: UpdateReview(). Product with id {review.ProductId} does not exist.");
                    return BadRequest($"Product with id {review.ProductId} does not exist.");
                }

                var existingReview = await _repository.GetReview(review.Id);
                if (existingReview == null)
                {
                    _logger.LogError("Server Error in ReviewController, method: UpdateReview(). Review not found.");
                    return NotFound();
                }

                var result = await _repository.UpdateReview(review);

                if (result == null)
                {
                    _logger.LogError("Server Error in ReviewController, method: UpdateReview(). Review update failed.");
                    return StatusCode(500);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: UpdateReview().", ex);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            try
            {
                await _repository.DeleteReview(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: DeleteReview().", ex);
                return StatusCode(500);
            }
        }

        [HttpGet("/api/reviews/product/{productId}")]
        public async Task<ActionResult<List<Review>>> GetReviewsByProductId(int productId, int? pageIndex, int? pageSize)
        {
            try
            {
                var userToken = await HttpContext.GetTokenAsync(Constants.AccessTokenClaimType);
                var product = _productClient.GetProduct(new ProductRequest() { ProductId = productId }, await GetHeaders(userToken));
                if (product == null)
                {
                    _logger.LogError($"Bad request in ReviewController, method: GetReviewsByProductId(). Product with id {productId} does not exist.");
                    return BadRequest($"Product with id {productId} does not exist.");
                }
                var reviews = await _repository.GetReviewsByProductId(productId, pageIndex, pageSize);

                if (reviews == null)
                {
                    _logger.LogError($"ReviewController, method: GetReviewsByProductId(). Product with id {productId} not found.");
                    return NotFound();
                }

                return Ok(reviews);

            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: GetReviewsByProductId().", ex);
                return StatusCode(500);
            }
        }

        [HttpDelete("api/reviews/product/{productId}")]
        public async Task<ActionResult> DeleteReviewsForProduct(int productId)
        {
            try
            {
                var userToken = await HttpContext.GetTokenAsync(Constants.AccessTokenClaimType);
                var product = _productClient.GetProduct(new ProductRequest() { ProductId = productId }, await GetHeaders(userToken));
                if (product == null)
                {
                    _logger.LogError($"Bad request in ReviewController, method: DeleteReviewsForProduct(). Product with id {productId} does not exist.");
                    return BadRequest($"Product with id {productId} does not exist.");
                }

                await _repository.DeleteReviewsForProduct(productId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: DeleteReviewsForProduct().", ex);
                return StatusCode(500);
            }
        }

        private async Task<Metadata> GetHeaders(string userToken)
        {
            var headers = new Metadata();
            var token = await DelegateAsync(userToken);
            headers.Add("Authorization", $"Bearer {token.AccessToken}");

            return headers;
        }

        private async Task<TokenResponse> DelegateAsync(string userToken)
        {
            var client = _httpClientFactory.CreateClient();
            var endpoint = _configuration["oidc:authority"];

            if (!endpoint.Trim().EndsWith("/"))
            {
                endpoint = $"{endpoint}/";
            }
            endpoint = $"{endpoint}connect/token";

            // send custom grant to token endpoint, return response
            return await client.RequestTokenAsync(new TokenRequest
            {
                Address = endpoint,
                GrantType = "delegation",

                ClientId = "Reviews",
                ClientSecret = "ReviewClient",

                Parameters = { { "scope", "productcatalog" }, { "token", userToken } }
            });
        }
    }
}
