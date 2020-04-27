using ProductCatalog.Grpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Repository.Extensions
{
    public static class ProductExtension
    {
        public static Models.Product ToModel(this ProductResponse productResponse)
        {
            return new Models.Product()
            {
                Id = productResponse.Id,
                Name = productResponse.Name,
                Description = productResponse.Description
            };
        }
    }
}
