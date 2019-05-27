using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OdeToFood.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OdeToFood
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<OdeToFoodDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddScoped<IRestaurantData, RestaurantData>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/AccessDenied/";
    })
    .AddOpenIdConnect(options =>
        {
            options.Authority = "https://localhost:44337/";
            options.RequireHttpsMetadata = true;
            options.ClientId = "odetofoodclient";
            options.ClientSecret = "secret";
            options.ResponseType = "code id_token";
            //options.CallbackPath = new PathString();
            //options.SignedOutCallbackPath = new PathString();
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.SaveTokens = true;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("address");
            options.Scope.Add("roles");
            options.GetClaimsFromUserInfoEndpoint = true;
            options.Events = new OpenIdConnectEvents()
            {
                OnTokenValidated = tokenValidatedContext =>
                {
                    var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;
                    var subjectClaim = identity.Claims.FirstOrDefault(x => x.Type == "sub");

                    var newClaimIdentity = new ClaimsIdentity(tokenValidatedContext.Scheme.Name, "given_name", "role");
                    newClaimIdentity.AddClaim(subjectClaim);

                    tokenValidatedContext.Principal = new ClaimsPrincipal(newClaimIdentity);

                    return Task.FromResult(0);
                },
                OnUserInformationReceived = userInformationReceivedContext =>
                {
                    userInformationReceivedContext.User.Remove("address");
                    return Task.FromResult(0);
                }
            };
        });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNodeModules(env);
            app.UseCookiePolicy();
            app.UseMvc();
        }

    }
}
