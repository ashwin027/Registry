using Reviews.Models;
using Reviews.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.Repository
{
    public interface IReviewRepository
    {
        Task<PaginatedList<Review>> GetReviews(int? pageNumber, int? pageSize);
        Task<Review> GetReview(int id);
        Task<PaginatedList<Review>> GetReviewsByProductId(int productId, int? pageNumber, int? pageSize);
        Task<Review> CreateReview(Review review);
        Task<Review> UpdateReview(Review review);
        Task DeleteReview(int id);
        Task DeleteReviewsForProduct(int productId);
    }
}
