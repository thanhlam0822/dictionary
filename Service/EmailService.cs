using MailKit.Security;
using MailKit;
using Microsoft.Extensions.Options;
using MimeKit;
using pvo_dictionary.Model;
using pvo_dictionary.Setting;
using pvo_dictionary.Repository;
using MailKit.Net.Smtp;
using pvo_dictionary.Data;
using System.Security.Cryptography;

namespace pvo_dictionary.Service
{
    public class EmailService : EmailRepository
    {
        private readonly MailSettings _mailSettings;
        private readonly DataContext dataContext;
        public EmailService(IOptions<MailSettings> mailSettings, DataContext dataContext)
        {
            _mailSettings = mailSettings.Value;
            this.dataContext = dataContext;
        }

       

        public async Task<ServiceResult> SendEmailAsync(MailRequest mailRequest)
        {   
            var user = dataContext.user.FirstOrDefault(u => u.email== mailRequest.username);
            if (user != null && BCrypt.Net.BCrypt.Verify(mailRequest.password, user.password)  && user.status == 0 )
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(user.email));
                email.Subject = "Active account request";
                var builder = new BodyBuilder();
                //builder.HtmlBody = "Click <a href=\"http://www.example.com\">here</a> to active the account .";
                builder.HtmlBody = "Click <a href=\"https://localhost:7260/api/account/active_account?token="+user.verify_token+"\">here</a> to active the account .";
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
            else
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Invalid account", 1002);
            }
            


        }
        public async Task<ServiceResult> ForgotPassword(string userEmail)
        {
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            if( user == null)
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Invalid account", 1002);
            }
            else 
            {
                user.reset_password_token = GenerateResetPasswordToken();
                dataContext.SaveChanges();
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(userEmail));
                email.Subject = "Reset password request";
                var builder = new BodyBuilder();
                builder.HtmlBody = "Click <a href=\"https://localhost:7260/api/account/reset-password?token=" + user.reset_password_token + "\">here</a> to active the account .";
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
        }
        public static string GenerateResetPasswordToken(int size = 32)
        {
            var rng = new RNGCryptoServiceProvider();
            var tokenData = new byte[size];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }
    }
}
