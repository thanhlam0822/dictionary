using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Security.Claims;

namespace pvo_dictionary.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UserController : ControllerBase

    {
        private readonly UserRepository userRepository;

        public UserController(UserRepository userRepository)
        {
            this.userRepository = userRepository;

        }
        [HttpPost("register")]
        public ServiceResult registerUser([FromBody] User user)
        {
            return userRepository.saveUser(user);
        }
        [HttpGet("active_account")]
        public ServiceResult activeAccount([FromQuery] string token)
        {

            return userRepository.activeAccount(token);
        }
        [HttpPost("login")]
        public ServiceResult login([FromForm] MailRequest request)
        {
            return userRepository.login(request);
        }
        [HttpPost("reset_password")]
        public ServiceResult resetPassword([FromBody] ResetPasswordRequestDTO dto) 
        {
            return userRepository.ResetPassword( dto);        
        }
        [HttpPost("update_password"),Authorize]
        public ServiceResult updatePassword([FromForm]UpdatePasswordDTO dto)
        {
            return userRepository.UpdatePassword(dto);
        }
        [HttpPatch("update_user_info"), Authorize]
        public ServiceResult updateUserInfo([FromForm] UpdateUserInfoRequest request)
        {
            return userRepository.UpdateUserInfo(request);
        }

    }

    }

