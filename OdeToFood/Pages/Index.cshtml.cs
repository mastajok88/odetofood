using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OdeToFood.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public async Task OnGet()
        {
            await WriteOutIdentityInfo();
        }

        public async Task WriteOutIdentityInfo()
        {
            string identityToken = await AuthenticationHttpContextExtensions.GetTokenAsync(HttpContext, OpenIdConnectDefaults.AuthenticationScheme, OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token: {identityToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        
    }
}
