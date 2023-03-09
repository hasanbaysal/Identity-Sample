using IdentityTutorial.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTutorial.Service.Services
{
    public interface IUserService
    {

        Task<(bool, bool,IEnumerable<IdentityError>?)> UserSignUpAsync(SignUpViewModel vm, ControllerBase controller);

        Task<bool> UserEmailActivationAsync(string userId, string token, ControllerBase controller);

        Task<(bool login, bool ısLock, bool emailConfirmed)> SignInUserAsync(SignInViewModel vm, ModelStateDictionary modelState);
        Task<bool> ForgetPasswordEmailAsync(ForgetPasswordViewModel vm, ControllerBase controller);
        Task<bool> ForgetPasswordEmailCheckAsync(string userId, string token);
        Task<(bool userNull, bool success, IEnumerable<IdentityError>? errors)> ResetPasswordAsync(ResetPasswordViewModel vm, string userId, string token);

    }
}
