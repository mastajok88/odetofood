using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Api.Authorization
{
    public class IsOwnerHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        //inject some repos to ctor to receive data from DB or etc
        public IsOwnerHandler()
        {

        }

        //example implementation
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsOwnerRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext != null)
            {
                context.Fail();
                return Task.FromResult(0);
            }
            //imagine Id of some subject
            var id = filterContext.RouteData.Values["id"].ToString();
            Guid someId;

            if (!Guid.TryParse(id, out someId))
            {
                context.Fail();
                return Task.FromResult(0);
            }
            //get sub claim
            var ownerId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            /*
            * call repo or service to check something in DB
            * 
            */

            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }
}
