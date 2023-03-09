using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IdentityTutorial.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            RoleNames = new List<string>();
        }
        public string? UserId { get; set; }
        public string? UserName { get; set; } 
        public string? Email { get; set; } 

        public List<string>? RoleNames { get; set; }

    }
}
