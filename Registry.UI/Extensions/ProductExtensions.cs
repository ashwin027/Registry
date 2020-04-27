using Registry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registry.UI.Extensions
{
    public static class ProductExtensions
    {
        public static ProductAggregate ToAggregate(this Product product, RegistryRecord registryRecord, bool isAdded)
        {
            return new ProductAggregate()
            {
                Id = product.Id,
                Quantity = registryRecord?.Quantity,
                Description = product.Description,
                Name = product.Name,
                RegistryId = registryRecord?.Id,
                IsAdded = isAdded
            };
        }
    }
}
