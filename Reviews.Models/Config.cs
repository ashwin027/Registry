using System;
using System.Collections.Generic;
using System.Text;

namespace Reviews.Models
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public string ProductEndpoint { get; set; }
        public Identity Identity { get; set; }
    }

    public class Identity
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
    }
}
