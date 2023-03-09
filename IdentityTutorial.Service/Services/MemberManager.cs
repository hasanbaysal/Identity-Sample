using IdentityTutorial.Core.ViewModels;
using IdentityTutorial.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTutorial.Service.Services
{
    public class MemberManager:IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _filProvider;
        public MemberManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider filProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _filProvider = filProvider;
        }

        public async Task<UserViewModel> GetUserViewModelByUserName(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

            var userViewModel = new UserViewModel()
            {

                PhoneNumer = currentUser!.PhoneNumber,
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Picture = currentUser.Picture

            };
            return userViewModel;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        
       public async Task<bool>  CheckPasswordAsync(string userName,string password)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser!, password);

            return checkOldPassword; 


        }

       public async Task<(bool,IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword,string newPassword)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(currentUser!, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser!, newPassword, true, false);

            return (true, null);

        }

        public async Task<UserEditViewModel> GetUserEditViewModelAsync(string userName)
        {

            var currentUser = (await _userManager.FindByNameAsync(userName))!;


            var data = new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Email = currentUser.Email!,
                Gender = currentUser.Gender,
                Phone = currentUser.PhoneNumber!
            };

            return data;
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> UpdateUserEditViewModelAsync(UserEditViewModel vm,string userName)
        {

            var currentUser = (await _userManager.FindByNameAsync(userName))!;
            currentUser.UserName = vm.UserName;
            currentUser.Email = vm.Email;
            currentUser.PhoneNumber = vm.Phone;
            currentUser.BirthDate = vm.BirthDate;
            currentUser.City = vm.City;
            currentUser.Gender = vm.Gender;


            if (vm.Picture != null && vm.Picture.Length > 0)
            {
                var wwwroot = _filProvider.GetDirectoryContents("wwwroot");
                var randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.Picture.FileName);
                var newPicture = Path.Combine(wwwroot!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);
                using var stream = new FileStream(newPicture, FileMode.Create);
                await vm.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;
            }

            var result = await _userManager.UpdateAsync(currentUser);

            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }



            if (currentUser.BirthDate != null)
            {
                await _userManager.UpdateSecurityStampAsync(currentUser);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInWithClaimsAsync(currentUser, false, new[] { new Claim("birthdate", currentUser.BirthDate!.ToString()!) });
               

                return (true,null);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, false);

            return (true, null);


        }


        public List<ClaimsViewModel> GetUserClaims(ClaimsPrincipal Userprincipal)
        {

            var data= Userprincipal.Claims.Select(x => new ClaimsViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();


            return data;
        }


    }
}
