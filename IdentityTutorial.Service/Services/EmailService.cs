using IdentityTutorial.Core.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IdentityTutorial.Service.Services
{
    public class EmailService : IEmailService
    {
        
        private  EmailSettings EmailSettings { get; set; }
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            EmailSettings = emailSettings.Value;
        }

        public async Task SendResetPasswordEmail(string resetLink, string ToEmail)
        {

            var smtpClient = new SmtpClient();
            smtpClient.Host =EmailSettings.Host; 
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(EmailSettings.Email, EmailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(EmailSettings.Email);
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Identity Tutorial App | Password Reset Link";
            mailMessage.Body = @$"<h4> click the link to reset your password </h4>
                                <p> your link .... <a href='{resetLink}'> password reset link </a></p>";
                                

            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }






        public async Task SendEmailActivationLink(string resetLink, string ToEmail)
        {

            var smtpClient = new SmtpClient();
            smtpClient.Host = EmailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(EmailSettings.Email, EmailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(EmailSettings.Email);
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Identity Tutorial App | Email Activation Link";
            mailMessage.Body = @$"<h4> Click the clink to activate your account </h4>
                                <p> your link .... <a href='{resetLink}'> account activation link </a></p>";


            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
