using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IRestaurantData _restaurantData;
        private readonly ILogger<ListModel> _logger;
        public string Address { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public string Message { get; set; }

        public IEnumerable<Restaurant> Restaurants { get; set; }

        public ListModel(IConfiguration configuration, IRestaurantData restaurantData, ILogger<ListModel> logger)
        {
            _configuration = configuration;
            _restaurantData = restaurantData;
            _logger = logger;
           
        }

        public async Task OnGet()
        {
            this.Message = _configuration["Message"];
            Restaurants = _restaurantData.GetAll();
            Address = await OrderFrame();
        }

        public async Task<string> OrderFrame()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44337/");

            var metadataResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(metadataResponse.UserInfoEndpoint);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception("Problem accessing the UserInfo endpoint", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(x => x.Type == "address")?.Value;


            return address;
        }

    }
}