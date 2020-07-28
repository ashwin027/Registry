using System;
using System.Collections.Generic;
using System.Text;

namespace Reviews.Shared
{
    public class IdentityConfiguration
    {
        public const string Oidc = "Oidc";
        public string Authority { get; set; }
        public string ApiName { get; set; }
    }
}
