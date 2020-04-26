using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }
}
