using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Reviews.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProductCatalog.Grpc.Product;

namespace Reviews.Grpc.Services
{
    public class ReviewService: Review.ReviewBase
    {
        private readonly ILogger<ReviewService> _logger;
        private readonly IReviewRepository _repository;
        private readonly ProductClient _productClient;
        public ReviewService(ILogger<ReviewService> logger, IReviewRepository reviewRepository, ProductClient productClient)
        {
            _logger = logger;
            _repository = reviewRepository;
            _productClient = productClient;
        }
        public override async Task<Reviews> GetAllReviews(ReviewsRequest request, ServerCallContext context)
        {
            Status status;
            try
            {
                var reviews = await _repository.GetReviews(request.PageIndex, request.PageSize);

                if (reviews == null)
                {
                    status = new Status(StatusCode.NotFound, $"Reviews not found.");
                    _logger.LogError($"ReviewService, method: GetAllReviews(). Reviews not found.");
                }
                else
                {
                    var reviewsResponse = new Reviews();
                    reviewsResponse.PageIndex = reviews.PageIndex;
                    reviewsResponse.TotalPages = reviews.TotalPages;
                    reviewsResponse.TotalCount = reviews.TotalCount;
                    reviewsResponse.Reviews_.Add(reviews.Select(p => new ReviewResponse
                    {
                        Id = p.Id,
                        Description = p?.Description,
                        ProductId = p.ProductId,
                        Rating = p.Rating,
                        SubmitDate = p.SubmitDate.ToUniversalTime().ToTimestamp(),
                        Title = p.Title
                    }));

                    return reviewsResponse;
                }

                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server Error in ReviewService,{context.Method}");
                throw new RpcException(Status.DefaultCancelled, ex.Message);
            }
        }
        public override async Task<ReviewResponse> GetReview(ReviewRequest request, ServerCallContext context)
        {
            Status status;
            try
            {
                var review = await _repository.GetReview(request.ReviewId);

                if (review == null)
                {
                    status = new Status(StatusCode.NotFound, $"Review with id {request.ReviewId} not found.");
                    _logger.LogError($"ReviewService, method: GetReview(). Review with id { request.ReviewId} not found.");
                }
                else
                {
                    var reviewResponse = new ReviewResponse()
                    {
                        Description = review.Description,
                        ProductId = review.ProductId,
                        Title = review.Title,
                        Id = review.Id,
                        Rating = review.Rating,
                        SubmitDate = review.SubmitDate.ToUniversalTime().ToTimestamp()
                    };
                    
                    return reviewResponse;
                }

                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server Error in ReviewService,{context.Method}");
                throw new RpcException(Status.DefaultCancelled, ex.Message);
            }
        }

        public override async Task<Reviews> GetReviewsForProduct(ReviewRequestForProduct request, ServerCallContext context)
        {
            Status status;
            try
            {
                var product = await _productClient.GetProductAsync(new ProductCatalog.Grpc.ProductRequest() { ProductId = request.ProductId });
                if (product == null)
                {
                    status = new Status(StatusCode.NotFound, $"Product with id {request.ProductId} not found.");
                    _logger.LogError($"ReviewService, method: GetReviewsForProduct(). Product with id { request.ProductId} not found.");
                }
                else
                {
                    var reviews = await _repository.GetReviewsByProductId(request.ProductId, request.PageIndex, request.PageSize);

                    if (reviews == null)
                    {
                        status = new Status(StatusCode.NotFound, $"Reviews for product with id {request.ProductId} not found.");
                        _logger.LogError($"ReviewService, method: GetReviewsForProduct(). Review for product with id {request.ProductId} not found.");
                    }
                    else
                    {
                        var reviewsResponse = new Reviews();
                        reviewsResponse.PageIndex = reviews.PageIndex;
                        reviewsResponse.TotalPages = reviews.TotalPages;
                        reviewsResponse.TotalCount = reviews.TotalCount;
                        reviewsResponse.Reviews_.Add(reviews.Select(p => new ReviewResponse
                        {
                            Id = p.Id,
                            Description = p?.Description,
                            ProductId = p.ProductId,
                            Rating = p.Rating,
                            SubmitDate = p.SubmitDate.ToUniversalTime().ToTimestamp(),
                            Title = p.Title
                        }));

                        return reviewsResponse;
                    }
                }

                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server Error in ReviewService,{context.Method}");
                throw new RpcException(Status.DefaultCancelled, ex.Message);
            }
        }
    }
}
