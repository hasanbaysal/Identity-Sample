using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Core.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;



        [Required(ErrorMessage = "Password Confirm is required")]
        [Display(Name = "Password Confirm")]
        [Compare(nameof(Password), ErrorMessage = "The Passwords are not same")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = null!;


    }
}
