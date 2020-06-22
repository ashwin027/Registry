using Grpc.Core;
using Microsoft.AspNetCore.Components.Authorization;
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
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public ReviewRepository(ReviewClient reviewClient, ILogger<RegistryRepository> logger, AuthenticationStateProvider AuthenticationStateProvider)
        {
            _reviewClient = reviewClient;
            _logger = logger;
            _authenticationStateProvider = AuthenticationStateProvider;
        }
        public async Task<PagedResult<Review>> GetReviewsForProduct(int productId, int? pageIndex, int? pageSize)
        {
            try
            {
                var headers = await GetHeaders();
                var reviewModels = new PagedResult<Models.Review>();
                var reviews = await _reviewClient.GetReviewsForProductAsync(new Reviews.Grpc.ReviewRequestForProduct() { PageIndex = pageIndex, PageSize = pageSize, ProductId = productId  }, headers);
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

        private async Task<Metadata> GetHeaders()
        {
            var headers = new Metadata();
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var accessToken = authState.User.Claims.FirstOrDefault(c => c.Type.Equals(Constants.AccessTokenClaimType));
            if (accessToken != null && !string.IsNullOrWhiteSpace(accessToken?.Value))
            {
                headers.Add("Authorization", $"Bearer {accessToken?.Value}");
            }

            return headers;
        }
    }
}
