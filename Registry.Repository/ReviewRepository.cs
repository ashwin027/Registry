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
        public ReviewRepository(ReviewClient reviewClient)
        {
            _reviewClient = reviewClient;
        }
        public async Task<List<Review>> GetReviewsForProduct(int productId, int? pageIndex, int? pageSize)
        {
            try
            {
                var reviewModels = new List<Models.Review>();
                var reviews = await _reviewClient.GetReviewsForProductAsync(new Reviews.Grpc.ReviewRequestForProduct() { PageIndex = pageIndex, PageSize = pageSize, ProductId = productId  });
                reviewModels.AddRange(reviews.Reviews_.Select(r => r.ToModel()));

                return reviewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
