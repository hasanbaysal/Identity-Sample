using IdentityTutorial.Core.ViewModels;
using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTutorial.Service.Services
{


    public class UsersManager:IUserService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }
        public  async Task<(bool,bool,IEnumerable<IdentityError>?)> UserSignUpAsync(SignUpViewModel vm,ControllerBase controller)
        {

            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = vm.UserName,
                PhoneNumber = vm.Phone,
                Email = vm.Email,

            }, vm.Password);


            if (!identityResult.Succeeded)
            {

                return (false,false, identityResult.Errors);
            }



            var _10DaysFreeAcces = new Claim("freeaccess10days", DateTime.Now.AddDays(10).ToString());
            var user = await _userManager.FindByNameAsync(vm.UserName);
            var claimAddResult = await _userManager.AddClaimAsync(user!, _10DaysFreeAcces);

            if (!claimAddResult.Succeeded)
            {
                return (false,false, claimAddResult.Errors);
            }


            if (!vm.EmailIgnore)
            {
                var newUser = await _userManager.FindByNameAsync(vm.UserName);
                var EmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser!);
                var mailLink = controller.Url.Action("AccountActivation", "Home", new { userId = newUser!.Id, token = EmailToken }, _httpContextAccessor!.HttpContext!.Request.Scheme);
                await _emailService.SendEmailActivationLink(mailLink!, vm.Email);
                return (true,true, null);
            }


            return(true,false, null);

        }
        public async Task<bool> UserEmailActivationAsync(string userId,string token,ControllerBase controller)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("user not found");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
               

                var EmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

                var mailLink = controller.Url.Action("AccountActivation", "Home", new { userId = user!.Id, token = EmailToken }, _httpContextAccessor!.HttpContext!.Request.Scheme);

                await _emailService.SendEmailActivationLink(mailLink!, user.Email!);


                return false;

            }

            return true;


        }
        public async Task<(bool login,bool ısLock, bool emailConfirmed)> SignInUserAsync(SignInViewModel vm, ModelStateDictionary modelState)
        {

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user == null)
            {
                modelState.AddModelError("", "user not found. check your email and password");
                return (false, false, false);
            }
            var signInResul = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

            if (signInResul.Succeeded)
            {

                if (user.BirthDate != null)
                {
                    await _signInManager.SignInWithClaimsAsync(user, vm.RememberMe, new[] { new Claim("birthdate", user.BirthDate!.ToString()!) });
                }

                if (!user.EmailConfirmed)
                {

                    return (true, false, false);
                }
                return (true, false, true);
               
            }

            if (signInResul.IsLockedOut)
            {
                modelState.AddModelError("", "You cannot login for 3 minutes. plase wait unilt ");
                return (false, true, false);
            }
            var count = await _userManager.GetAccessFailedCountAsync(user);
            modelState.AddModelError("", "check your email and password");
            modelState.AddModelError("", $"{count} failed login attempt");
            return(false, false, false);



        }

        public async Task<bool> ForgetPasswordEmailAsync(ForgetPasswordViewModel vm,ControllerBase controller)
        {

            var hasUser = await _userManager.FindByEmailAsync(vm.Email);

            if (hasUser == null)
            {

                return false;
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var resetLink = controller.Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, token = token }, _httpContextAccessor!.HttpContext!.Request.Scheme);

            await _emailService.SendResetPasswordEmail(resetLink!, vm.Email);
            return true;
            
        }

        public async Task<bool> ForgetPasswordEmailCheckAsync(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            IdentityOptions a = new IdentityOptions();
            var result = await _userManager.VerifyUserTokenAsync(user, a.Tokens.PasswordResetTokenProvider, UserManager<AppUser>.ResetPasswordTokenPurpose, token).ConfigureAwait(false);

            if (!result)
            {
                return false;
            }

            return true;
        }
        
        public async Task<(bool userNull, bool success,  IEnumerable<IdentityError>? errors)> ResetPasswordAsync(ResetPasswordViewModel vm,string userId,string token)
        {
            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
           
            if(hasUser == null) return (true,false,null);

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, vm.PasswordConfirm);

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(hasUser);
                return (false, true, null);
            }

            return (false, false, result.Errors);

        }

    }
}
