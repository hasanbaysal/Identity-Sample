using IdentityTutorial.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Core.ViewModels
{
    public class UserEditViewModel
    {


        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;


        [EmailAddress(ErrorMessage = "Check your email address format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = null!;
     
        [Display(Name = "BirthDate")]
        public DateTime? BirthDate { get; set; }
        
        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name ="Picture")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Picture")]
        public Gender? Gender { get; set; }
    }
}
