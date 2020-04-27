using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Models
{
    public class ProductAggregate: Product
    {
        public int? Quantity { get; set; }
        public bool IsAdded { get; set; }
        public int? RegistryId { get; set; }
    }
}
