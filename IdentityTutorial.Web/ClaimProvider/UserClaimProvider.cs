using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityTutorial.Web.ClaimProvider
{


    public class UserClaimProvider : IClaimsTransformation
    {
      

        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            var identityUser = principal.Identity as ClaimsIdentity;

            var currentUser = await _userManager.FindByNameAsync(identityUser!.Name!);

          
            if (string.IsNullOrEmpty(currentUser!.City))
            {
                return principal;
            }

            if (!principal.Claims.Any(x=>x.Type =="city"))
            {
                Claim cityClaim = new Claim("city",currentUser!.City!.ToLower());
                identityUser.AddClaim(cityClaim);

            }
            return principal;

        }
    }
}
