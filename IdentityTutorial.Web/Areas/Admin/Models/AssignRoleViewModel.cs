namespace IdentityTutorial.Web.Areas.Admin.Models
{
    public class AssignRoleViewModel
    {
        public string Id { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool Exist { get; set; }
    }
}
