using MailKit;
using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;

namespace pvo_dictionary.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly EmailRepository mailService;
        public MailController(EmailRepository mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send_active_email")]
        public async Task<ServiceResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
               return await mailService.SendEmailAsync(request);
               
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpGet("forgot_password")]
        public async Task<ServiceResult> SendResetPasswordEmail(string userEmail)
        {
            try
            {
                return await mailService.ForgotPassword(userEmail);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
