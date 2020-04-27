using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Models
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public string ProductEndpoint { get; set; }
        public string ReviewEndpoint { get; set; }
    }
}
