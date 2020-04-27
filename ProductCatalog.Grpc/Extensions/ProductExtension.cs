using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Grpc.Extensions
{
    public static class ProductExtension
    {
        public static ProductResponse ToGrpcModel(this Models.Entities.Product product)
        {
            return new ProductResponse()
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name
            };
        }
    }
}
