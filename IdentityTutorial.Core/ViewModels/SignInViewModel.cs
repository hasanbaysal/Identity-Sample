using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Core.ViewModels
{
    public class SignInViewModel
    {
       

        [EmailAddress(ErrorMessage = "Check your email address format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;



        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
