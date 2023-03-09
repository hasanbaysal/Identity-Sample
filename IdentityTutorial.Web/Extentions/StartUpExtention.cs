using IdentityTutorial.Web.IdentityCustomValidations;
using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IdentityTutorial.Web.Extentions
{
    public static class StartUpExtention
    {


        
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, AppRole>(options =>
            {

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefgiklmnhoprstuvywxyzq1234567890_";  


                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); 
                options.Lockout.MaxFailedAccessAttempts = 3; 


                options.Password.RequiredLength = 6; 
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequireLowercase = true; 
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false; 

            }).AddPasswordValidator<CustomPasswordValidator>()
                 .AddUserValidator<CustomUserValidation>()
                 .AddErrorDescriber<CustomErrorDescriber>()
                 .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();

        }
    }
}
