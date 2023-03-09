using IdentityTutorial.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTutorial.Service.Services
{
    public interface IMemberService
    {
        Task<UserViewModel> GetUserViewModelByUserName(string userName);
        Task Logout();

        Task<bool> CheckPasswordAsync(string userName, string password);

        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
        Task<UserEditViewModel> GetUserEditViewModelAsync(string userName);

        Task<(bool, IEnumerable<IdentityError>?)> UpdateUserEditViewModelAsync(UserEditViewModel vm, string userName);


        List<ClaimsViewModel> GetUserClaims(ClaimsPrincipal Userprincipal);
    }
}
