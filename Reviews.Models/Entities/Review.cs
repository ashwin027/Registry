using System;
using System.Collections.Generic;
using System.Text;

namespace Reviews.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
