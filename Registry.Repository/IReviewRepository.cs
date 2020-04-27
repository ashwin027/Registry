using Registry.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Repository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsForProduct(int productId, int? pageIndex, int? pageSize);        
    }
}
