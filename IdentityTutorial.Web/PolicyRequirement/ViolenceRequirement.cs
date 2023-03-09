using Microsoft.AspNetCore.Authorization;

namespace IdentityTutorial.Web.PolicyRequirement
{
    public class ViolenceRequirement:IAuthorizationRequirement
    {
        public int ThresOldAge { get; set; }
    }
    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {

            if (!context.User.HasClaim(x=>x.Type=="birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var birthDateClaim = context.User.Claims.First(x => x.Type == "birthdate");
            var birtDate = Convert.ToDateTime(birthDateClaim.Value);
            var today = DateTime.Now;
            var age = today.Year - birtDate.Year;


            if(birtDate > today.AddYears(-age)) age--;

            
            if (requirement.ThresOldAge > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;

        }
    }

}
