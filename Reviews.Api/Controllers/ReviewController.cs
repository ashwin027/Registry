using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reviews.Models.Entities;
using Reviews.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reviews.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ReviewController: ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewRepository _repository;
        public ReviewController(ILogger<ReviewController> logger, IReviewRepository reviewRepository)
        {
            _logger = logger;
            _repository = reviewRepository;
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


        // TODO add pagination
        [HttpGet("/api/reviews")]
        public async Task<ActionResult<List<Review>>> GetReviews()
        {
            try
            {
                var reviews = await _repository.GetReviews();

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

        // TODO: Add validation to make sure the product exists.
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            try
            {
                if (review == null)
                {
                    _logger.LogError("Bad request in ReviewController, method: CreateReview(). Request is null.");
                    return BadRequest();
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

        // TODO add pagination
        [HttpGet("api/reviews/product/{productId}")]
        public async Task<ActionResult<List<Review>>> GetReviewsByProductId(int productId)
        {
            try
            {
                var reviews = await _repository.GetReviewsByProductId(productId);

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
                await _repository.DeleteReviewsForProduct(productId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error in ReviewController, method: DeleteReviewsForProduct().", ex);
                return StatusCode(500);
            }
        }
    }
}
