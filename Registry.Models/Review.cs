using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
