
using IdentityTutorial.Web.Areas.Admin.Models;
using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace IdentityTutorial.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public HomeController(UserManager<AppUser> userManager, AppDbContext context, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserList()
        {


            var UserWithRoles = _context.UserRoles.ToList();



            var data = await _userManager.Users.ToListAsync();

            var roles = await _roleManager.Roles.ToListAsync();

            var UserIdWithRoleID = await _context.UserRoles.ToListAsync();



            var userWithoutRole = new List<UserViewModel>();



            data.ForEach(x =>
            {
                if (UserIdWithRoleID.Where(y => y.UserId == x.Id).Count() > 0)
                {

                }
                else
                {
                    userWithoutRole.Add(new()
                    {
                        UserId = x.Id,
                        UserName=x.UserName,
                        Email=x.Email,

                    });
                }


            });


            var x = UserWithRoles.Join(roles,
                               UserWithRoles => UserWithRoles.RoleId,
                               roles => roles.Id,
                               (UserWithRoles, roles) => new
                               {
                                   roleId = roles.Id,
                                   rolName = roles.Name,
                                   UserId = UserWithRoles.UserId
                               });

            var list = x.Join(data,
                        x => x.UserId,
                        data => data.Id,
                        (x, data) => new
                        {
                            UserName = data.UserName,
                            RoleName = x.rolName,
                            UserId = x.UserId,
                            Email = data.Email,

                        });

            var result = list.GroupBy(x => x.UserId, x => x.RoleName
                    ,(key, g) =>
                    new UserViewModel()
                    {
                        UserId =key,
                        RoleNames=g.ToList()!,
                        Email =list.Where(x=>x.UserId==key).Select(x=>x.Email).First(),
                        UserName= list.Where(x => x.UserId == key).Select(x => x.UserName).First()
                    }).ToList();


            result.AddRange(userWithoutRole);

            return View(result);
        }
    }
}
