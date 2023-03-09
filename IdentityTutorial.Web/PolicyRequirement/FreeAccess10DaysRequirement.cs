using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualBasic;

namespace IdentityTutorial.Web.PolicyRequirement
{
    public class FreeAccess10DaysRequirement : IAuthorizationRequirement
    {
    }
    public class FreeAccess10DaysRequirementHandler : AuthorizationHandler<FreeAccess10DaysRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FreeAccess10DaysRequirement requirement)
        {

            var accesClaim = context.User.HasClaim(x => x.Type == "freeaccess10days");

            if (!accesClaim) 
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var claim = (context.User.FindFirst(x => x.Type == "freeaccess10days"))!;

            if (  DateTime.Now > Convert.ToDateTime(claim.Value))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;

        }
    }

}
