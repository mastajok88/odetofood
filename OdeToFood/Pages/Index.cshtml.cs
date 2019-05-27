using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OdeToFood.Services;

namespace OdeToFood.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOdeToFoodHttpClient _odeToFoodHttpClient;

        public IEquatable<string> DefaultResponse { get; set; } = string.Empty;

        public IndexModel(IOdeToFoodHttpClient odeToFoodHttpClient)
        {
            _odeToFoodHttpClient = odeToFoodHttpClient;
        }

        public async Task<IActionResult> OnGet()
        {
            await WriteOutIdentityInfo();

            var httpClient = await _odeToFoodHttpClient.GetClient();
            var response = await httpClient.GetAsync("api/values").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                DefaultResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                {
                    return RedirectToPage("/AccessDenied");
                }
            }


            return Page();
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
