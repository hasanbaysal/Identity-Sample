
using IdentityTutorial.Repository.Models;
using IdentityTutorial.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;   
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using IdentityTutorial.Core.Models;
using System.Runtime.CompilerServices;
using IdentityTutorial.Service.Services;

namespace IdentityTutorial.Web.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
      
        private readonly IMemberService _memberService;
        private string UserName => User.Identity!.Name!;

        public MemberController(IMemberService memberService)
        {
           
            _memberService = memberService;
        }
        public async Task<IActionResult> Index() =>  View(await _memberService.GetUserViewModelByUserName(UserName));
        public async Task Logout() => await _memberService.Logout();
        public IActionResult PasswordChange() =>  View();
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel vm)
        {

            if (!ModelState.IsValid) { return View(); }
            
            if ( ! await _memberService.CheckPasswordAsync(UserName, vm.PasswordOld))
            {
                ModelState.AddModelError("", "Pleasse check your current password it isn't match ");
                return View();
            }
            var (succeeded, errors) = await _memberService.ChangePasswordAsync(UserName, vm.PasswordOld, vm.PasswordNew);

            if(!succeeded)
            {
                errors!.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
                return View();
            }

            TempData["success"] = "password change successful";

            return View();
        }
        public async Task<IActionResult> UserEdit()
        {

            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
            return View( await _memberService.GetUserEditViewModelAsync(UserName));
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel vm)
        {
            if (!ModelState.IsValid) { return View(); }
            var (result,errors) =await _memberService.UpdateUserEditViewModelAsync(vm, UserName);
            if (!result)
            {
                errors!.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
                return View();
            }
            TempData["successMessage"] = "user information updated ";
            return RedirectToAction("UserEdit");
        }
        public  IActionResult AccesDenied()
        {
            ViewBag.message = "you do not have permission to access this page";

            return View();
        }
        public   IActionResult Claims() => View(_memberService.GetUserClaims(User));

        [Authorize(Policy = "CityPolicy")]
        public IActionResult CityPolicyExample() => View();

        [Authorize(Policy = "FreeAccessPolicy")]
        public IActionResult FreeAccess10Days() => View();

        [Authorize(Policy = "ViolentContent")]
        public IActionResult ViolentContent() =>View();


    }
}
