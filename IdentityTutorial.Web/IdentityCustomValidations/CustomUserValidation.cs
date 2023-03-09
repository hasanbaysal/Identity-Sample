using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityTutorial.Web.IdentityCustomValidations
{
    public class CustomUserValidation : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {

            var errros = new List<IdentityError>();
            var isNumeric = int.TryParse(user.UserName![0].ToString(), out _);
            if (isNumeric)
            {
                errros.Add(new() { Code = "", Description = "The first character of the username cannot be a numeric value" });

            }
            if (errros.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errros.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
