using AutoMapper.Internal;
using pvo_dictionary.Model;

namespace pvo_dictionary.Repository
{
    public interface EmailRepository
    {
        Task<ServiceResult> SendEmailAsync(MailRequest mailRequest);
        Task<ServiceResult> ForgotPassword(string email);
    }
}
