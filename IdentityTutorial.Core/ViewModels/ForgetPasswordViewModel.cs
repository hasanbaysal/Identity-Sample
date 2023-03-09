using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Core.ViewModels
{
    public class ForgetPasswordViewModel
    {

        [EmailAddress(ErrorMessage = "Check your email address format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;


     
    }
}
