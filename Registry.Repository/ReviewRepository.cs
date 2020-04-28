using Microsoft.Extensions.Logging;
using Registry.Models;
using Registry.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reviews.Grpc.Review;

namespace Registry.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewClient _reviewClient;
        private readonly ILogger<RegistryRepository> _logger;
        public ReviewRepository(ReviewClient reviewClient, ILogger<RegistryRepository> logger)
        {
            _reviewClient = reviewClient;
            _logger = logger;
        }
        public async Task<PagedResult<Review>> GetReviewsForProduct(int productId, int? pageIndex, int? pageSize)
        {
            try
            {
                var reviewModels = new PagedResult<Models.Review>();
                var reviews = await _reviewClient.GetReviewsForProductAsync(new Reviews.Grpc.ReviewRequestForProduct() { PageIndex = pageIndex, PageSize = pageSize, ProductId = productId  });
                reviewModels.TotalCount = reviews.TotalCount;
                reviewModels.TotalPages = reviews.TotalPages;
                reviewModels.PageIndex = reviews.PageIndex;
                reviewModels.Data.AddRange(reviews.Reviews_.Select(r => r.ToModel()));

                return reviewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetReviewsForProduct().", ex);
                throw ex;
            }
        }
    }
}
