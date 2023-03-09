using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityTutorial.Web.IdentityCustomValidations
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!) )
            {
                errors.Add(new() { Code = "2", Description = "password cannot contain username" });
            }
            if (password!.ToLower().StartsWith("1234"))
            {
                errors.Add(new() { Code = "1", Description = "password cannot start with 1234" });
            }

            if (errors.Any())   
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
