using IdentityTutorial.Repository.Models;
using IdentityTutorial.Service.Services;
using IdentityTutorial.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityTutorial.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IUserService _usersManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IUserService usersManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _usersManager = usersManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        //kayıt ol
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel vm)
        {

            if (!ModelState.IsValid) { return View(); }
         
            var (operation, emailActivation, errors) = await _usersManager.UserSignUpAsync(vm,this);    

            if (!operation)
            {
                errors!.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
                return View();
            }
            
            if(emailActivation)
            {
                TempData["MessageSignUp"] = "the activation e-mail has been sent to your e-mail address.";
                 return RedirectToAction("Index");
            }

            TempData["MessageSignUp"] = "success";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AccountActivation(string userId,string token)
        {

            var activationResult = await _usersManager.UserEmailActivationAsync(userId, token, this);
            if (!activationResult)
            {
                TempData["MessageSignUp"] = " invalid link !!! Activation email sent again to your email address";
                return RedirectToAction("Index");
            }
            TempData["MessageSignUp"] = "Your email has been successfully verified ";
            return RedirectToAction("Index");
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel vm, string? returnUrl = null)
        {
            returnUrl ??= Url.Action("Index", "Home");
            var (login,isLock,emailConfirmed) = await _usersManager.SignInUserAsync(vm, this.ModelState);
            if (login)
            {
                if (!emailConfirmed)
                {
                    TempData["MessageSignUp"] = "please verify your e-mail address. check your inbox";
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl!);
            }
            if(isLock)
            {
                return View();
            }
            return View(vm);
        }

        public IActionResult ForgetPassword() => View();
     
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel vm)
        {

            if (!ModelState.IsValid) { return View(vm); }
           
            var result = await _usersManager.ForgetPasswordEmailAsync(vm, this);
           
            if (!result)
            {
                TempData["notvalid"] = "please enter a valid e-mail address";
                return RedirectToAction("ForgetPassword");
            }
          
            TempData["success"] = "password reset link send to your email";
           
            return RedirectToAction("ForgetPassword");
        }

       
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;


            var result =  await _usersManager.ForgetPasswordEmailCheckAsync(userId, token);

            if (result)
            {
                return View();
            }

            TempData["linkError"] = "your password link is invalid please request link again";
            return RedirectToAction("Index");

            #region token check

            //IdentityOptions a = new IdentityOptions();
            //var result = await _userManager.VerifyUserTokenAsync(user, a.Tokens.PasswordResetTokenProvider, UserManager<AppUser>.ResetPasswordTokenPurpose, token).ConfigureAwait(false);

            ////_userManager.ResetPasswordAsync(,)
            //if (!result)
            //{

            //    TempData["linkError"] = "your password link is invalid please request link again";

            //    return RedirectToAction("Index");
            //}


            #endregion

        }

        [HttpPost] 
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null) throw new Exception();

            if (!ModelState.IsValid)
            {
                TempData["userId"] = userId;
                TempData["token"] = token;
                return View(vm);
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            var (userNull,success,errors) = await _usersManager.ResetPasswordAsync(vm, userId.ToString()!, token.ToString()!);

            if (userNull)
            {
                TempData["linkError"] = "user not found";
                return RedirectToAction("Index");
            }

            if (success)
            {
                TempData["MessageSignUp"] = "Password reset successful";
                return RedirectToAction("Index");
            }

            errors!.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View(vm);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        public IActionResult Privacy() => View();



    }
}