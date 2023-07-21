using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace pvo_dictionary.Repository
{
    public interface UserRepository
    {
        List<User> GetAll();
        ServiceResult saveUser(User user);
        public ServiceResult activeAccount(string token);
        public ServiceResult login(MailRequest request);
        public ServiceResult ResetPassword(ResetPasswordRequestDTO dto);
        public ServiceResult UpdatePassword(UpdatePasswordDTO dto);
        public ServiceResult UpdateUserInfo(UpdateUserInfoRequest request);
        


    }
}
