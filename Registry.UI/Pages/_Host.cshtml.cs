using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registry.UI.Pages
{
    public class HostAuthenticationModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = await HttpContext.GetTokenAsync(Constants.AccessTokenClaimType);
                AccessToken = token;
            }
            return Page();
        }

        public string AccessToken { get; set; }
    }
}
