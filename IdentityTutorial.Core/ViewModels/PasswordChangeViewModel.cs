using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Core.ViewModels
{
    public class PasswordChangeViewModel
    {


        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
        public string PasswordOld { get; set; } = null!;


        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
        public string PasswordNew { get; set; } = null!;



        [Required(ErrorMessage = "Password Confirm is required")]
        [Display(Name = "New Password Confirm")]
        [Compare(nameof(PasswordNew), ErrorMessage = "The Passwords are not same")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
        public string PasswordNewConfirm { get; set; } = null!;
    }
}
