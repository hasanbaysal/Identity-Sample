using IdentityTutorial.Web.Areas.Admin.Models;
using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace IdentityTutorial.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

           var data=  await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                RoleName = x.Name!
            }).ToListAsync();


            return View(data);
        }
      
        
        [Authorize(Roles ="role-action")]
        public IActionResult RoleCreate()
        {
            return View();
        }

     
        
        [Authorize(Roles = "role-action")]
        [HttpPost]
        public  async Task<IActionResult> RoleCreate(RoleCreateViewModel vm)
        {
            if (!ModelState.IsValid) { return View(); }
            var result = await _roleManager.CreateAsync(new AppRole() { Name = vm.RoleName });
            if(!result.Succeeded)
            {
                result.Errors.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
                return View();
            }
            TempData["success"] = "role create successful";
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "role-action")]
        public async Task<IActionResult>  RoleUpdate(string id)
        {
            var role =( await _roleManager.FindByIdAsync(id))!;

            if (role == null) {

                throw new Exception("Role Not Found");
                
            }

            var UpdateRole = new RoleUpdateViewModel()
            {
                Id = role.Id,
                RoleName = role.Name!
            };


            return View(UpdateRole);
        }
        
        
        [Authorize(Roles = "role-action")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel vm)
        {

            var role = (await _roleManager.FindByIdAsync(vm.Id))!;

            if (role == null)
            {
                throw new Exception("Role Not Found");
            }

            role.Name = vm.RoleName;

            await _roleManager.UpdateAsync(role);


            TempData["success"] = "role update successful";
            return RedirectToAction("Index");
        }

      
        
        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleDelete(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                throw new Exception("role not found");
            }
            var result =await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("role delete error");
            }
            TempData["success"] = "role delete successful";
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> AssingRoleToUser(string id)
        {

            var currentUser = await _userManager.FindByIdAsync(id);
            if (currentUser == null)
            {
                throw new Exception("user not found error");
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var roleViewModel = new List<AssignRoleViewModel>();


            foreach (var role in roles)
            {
                roleViewModel.Add(new()
                {
                    Id = role.Id,
                    RoleName = role.Name!,
                    Exist = userRoles.Contains(role.Name!)
                });
            }
            ViewBag.id = id;
            return View(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssingRoleToUser(List<AssignRoleViewModel> vm,string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                throw new Exception("user not found");
            }


            foreach (var item in vm)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }

            }
            return RedirectToAction("UserList", "Home");
        }

    }
}
