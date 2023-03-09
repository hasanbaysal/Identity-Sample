using System.ComponentModel.DataAnnotations;

namespace IdentityTutorial.Web.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Role name is required")]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; } = null!;
    }
}
