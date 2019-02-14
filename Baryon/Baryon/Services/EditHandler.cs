using Baryon.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Baryon.Services
{
    public class EditHandler : AuthorizationHandler<EditRequirements, FAccess>
    {
        protected override Task HandleRequirementAsync(
           AuthorizationHandlerContext context,
           EditRequirements requirement,
           FAccess Forum)
        {

            if (Forum.UID == context.User.FindFirst(ClaimTypes.Name).Value || context.User.IsInRole("ADMIN"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}