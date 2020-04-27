using Registry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registry.UI.Extensions
{
    public static class ProductExtensions
    {
        public static ProductAggregate ToAggregate(this Product product, int? quantity)
        {
            var productAggregate = (ProductAggregate)product;
            productAggregate.Quantity = quantity;

            return productAggregate;
        }
    }
}
