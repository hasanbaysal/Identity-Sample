using Microsoft.AspNetCore.Identity;

namespace IdentityTutorial.Web.IdentityCustomValidations
{
    public class CustomErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {

            return new() { Code = "", Description = $"This {userName} userName is already registered in the system." };

        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "", Description = $"This {email} E-mail is already registered in the system." };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "", Description = "Password must be at least 6 digits. re-enter your password" };

        }
    }
}
