using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Registry.UI
{
    public class AppBase: ComponentBase
    {
        [Parameter]
        public string AccessToken { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var identity = authState.User.Identities.FirstOrDefault();
            identity.AddClaim(new Claim(OpenIdConnectParameterNames.AccessToken, AccessToken));
        }
    }
}
