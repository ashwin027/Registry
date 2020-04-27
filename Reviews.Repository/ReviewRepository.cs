using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reviews.Models;
using Reviews.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ILogger<ReviewRepository> _logger;
        private readonly ReviewContext _reviewContext;
        private const int defaultPageIndex = 1;
        private const int defaultPageSize = 5;
        public ReviewRepository(ReviewContext dbContext, ILogger<ReviewRepository> logger)
        {
            _reviewContext = dbContext;
            _logger = logger;
        }
        public async Task<Review> CreateReview(Review review)
        {
            try
            {
                _reviewContext.Reviews.Add(review);
                await _reviewContext.SaveChangesAsync();

                return review;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in CreateReview().", ex);
                throw ex;
            }
        }

        public async Task DeleteReview(int id)
        {
            try
            {
                var review = await _reviewContext.Reviews.FindAsync(id);
                _reviewContext.Reviews.Remove(review);
                await _reviewContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in DeleteReview().", ex);
                throw ex;
            }
        }

        public async Task<PaginatedList<Review>> GetReviews(int? pageNumber, int? pageSize)
        {
            try
            {
                IQueryable<Review> reviews = from r in _reviewContext.Reviews
                                               select r;

                return await PaginatedList<Review>.CreateAsync(reviews.AsNoTracking(), pageNumber ?? defaultPageIndex, pageSize ?? defaultPageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetReviews().", ex);
                throw ex;
            }
        }

        public async Task<Review> GetReview(int id)
        {
            try
            {
                return await _reviewContext.Reviews.FirstOrDefaultAsync(query => query.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetReview().", ex);
                throw ex;
            }
        }

        public async Task<PaginatedList<Review>> GetReviewsByProductId(int productId, int? pageNumber, int? pageSize)
        {
            try
            {
                IQueryable<Review> reviews = from r in _reviewContext.Reviews
                                             where r.ProductId == productId
                                             select r;

                return await PaginatedList<Review>.CreateAsync(reviews.AsNoTracking(), pageNumber ?? defaultPageIndex, pageSize ?? defaultPageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in GetReviewsByProductId().", ex);
                throw ex;
            }
        }

        public async Task<Review> UpdateReview(Review review)
        {
            try
            {
                if (review?.Id == null)
                {
                    return null;
                }

                var existingReview = await _reviewContext.Reviews.FindAsync(review.Id);
                if (existingReview == null)
                {
                    return null;
                }
                existingReview.Id = review.Id;
                existingReview.ProductId = review.ProductId;
                existingReview.Rating = review.Rating;
                existingReview.SubmitDate = review.SubmitDate;
                existingReview.Title = review.Title;
                existingReview.Description = review.Description;

                await _reviewContext.SaveChangesAsync();

                return existingReview;
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in UpdateReview().", ex);
                throw ex;
            }
        }

        public async Task DeleteReviewsForProduct(int productId)
        {
            try
            {
                var reviews = _reviewContext.Reviews.Where(r => r.ProductId == productId);
                _reviewContext.Reviews.RemoveRange(reviews);
                await _reviewContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in DeleteReviewsForProduct().", ex);
                throw ex;
            }
        }
    }
}
