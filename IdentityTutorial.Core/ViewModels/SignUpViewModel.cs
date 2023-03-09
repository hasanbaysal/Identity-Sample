using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Core.ViewModels
{
    public class SignUpViewModel
    {
      
        [Required(ErrorMessage = "User name is required")]
        [Display(Name ="User Name")]
        public string UserName { get; set; } = null!;


        [EmailAddress(ErrorMessage ="Check your email address format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = null!;


        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
        public string Password { get; set; } = null!;



        [Required(ErrorMessage = "Password Confirm is required")]
        [Display(Name = "Password Confirm")]
        [Compare(nameof(Password), ErrorMessage ="The Passwords are not same")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
        public string PasswordConfirm { get; set; } = null!;

        public bool  EmailIgnore { get; set; }

    }

}
