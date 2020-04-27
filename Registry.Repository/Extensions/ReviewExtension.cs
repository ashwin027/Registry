using Reviews.Grpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Repository.Extensions
{
    public static class ReviewExtension
    {
        public static Models.Review ToModel(this ReviewResponse reviewResponse)
        {
            return new Models.Review()
            {
                Description = reviewResponse.Description,
                Id = reviewResponse.Id,
                Rating = reviewResponse.Rating,
                SubmitDate = reviewResponse.SubmitDate.ToDateTime(),
                Title = reviewResponse.Title
            };
        }
    }
}
