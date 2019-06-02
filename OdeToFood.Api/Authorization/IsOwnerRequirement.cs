using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Api.Authorization
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
        public IsOwnerRequirement() {


        }
    }
}
