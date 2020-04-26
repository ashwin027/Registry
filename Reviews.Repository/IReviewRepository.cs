using Reviews.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.Repository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviews(int pageNumber = 1, int pageSize = 5);
        Task<Review> GetReview(int id);
        Task<List<Review>> GetReviewsByProductId(int productId, int pageNumber = 1, int pageSize = 5);
        Task<Review> CreateReview(Review review);
        Task<Review> UpdateReview(Review review);
        Task DeleteReview(int id);
        Task DeleteReviewsForProduct(int productId);
    }
}
