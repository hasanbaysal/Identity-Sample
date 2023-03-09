
namespace IdentityTutorial.Service.Services
{
    public interface IEmailService
    {
        
        Task SendResetPasswordEmail(string resetLink, string ToEmail );
        Task SendEmailActivationLink(string resetLink, string ToEmail );

        
    }
  
}
