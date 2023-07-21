using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using MySqlConnector;

using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace pvo_dictionary.Service
{
    public class UserService : UserRepository
    {
        private readonly DataContext dataContext;

        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        public UserService(DataContext dataContext, IConfiguration config, IHttpContextAccessor httpContextAccessor, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this.dataContext = dataContext;
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));

        }

        public List<User> GetAll()
        {
            return dataContext.user.ToList();
        }

        public static string GenerateVerifyToken(int size = 32)
        {
            var rng = new RNGCryptoServiceProvider();
            var tokenData = new byte[size];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.email),

            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public ServiceResult saveUser(User user)
        {
            try
            {
                user.user_id = Guid.NewGuid();
                user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
                user.verify_token = GenerateVerifyToken();

                bool exist = dataContext.user.SingleOrDefault(x => x.user_id == user.user_id) != null;

                if (exist)
                {

                }

                dataContext.user.Add(user);
                dataContext.SaveChanges();
                ServiceResult result = new ServiceResult(ServiceResultStatus.Success, null, null, null);
                return result;
            }
            catch (DbUpdateException ex)

            {
                ServiceResult result = null;
                if (ex.InnerException is MySqlConnector.MySqlException mysqlEx && mysqlEx.Number == 1062)
                {
                    result = new ServiceResult(ServiceResultStatus.Fail, null, ex.InnerException.Message, 1001);

                }

                return result;

            }
            catch (Exception ex)
            {
                ServiceResult result = new ServiceResult(ServiceResultStatus.Exception, null, ex.InnerException.Message, null);
                return result;
            }
        }
        public ServiceResult activeAccount(string token)
        {
            ServiceResult serviceResult;
            var user = dataContext.user.FirstOrDefault(u => u.verify_token == token);
            if (user == null)
            {
                serviceResult = new ServiceResult(ServiceResultStatus.Fail, null, "Invalid token", 1002);
            }
            else
            {
                user.status = 1;
                dataContext.SaveChanges();
                serviceResult = new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }

            return serviceResult;
        }

        public ServiceResult login(MailRequest request)
        {
            httpContextAccessor.HttpContext.Session.Remove("CurrentDictionaryID");
            ServiceResult serviceResult;
            var user = dataContext.user.FirstOrDefault(u => u.email == request.username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.password))
            {
                serviceResult = new ServiceResult(ServiceResultStatus.Fail, null, "Incorrect email or password", 1000);
            }
            else if (user != null && user.status == 0 && BCrypt.Net.BCrypt.Verify(request.password, user.password))
            {
                serviceResult = new ServiceResult(ServiceResultStatus.Fail, null, "Unactivated account", 1004);
            }

            else
            {
                string token = GenerateToken(user);
                var dictionary = dataContext.dictionary.Where(d => d.user_id == user.user_id).OrderByDescending(d => d.last_view_at).FirstOrDefault();
                LoginResponseDTO dto = new LoginResponseDTO(token, user.user_id, user.user_name, user.display_name, user.avatar, dictionary.dictionary_id, dictionary.dictionary_name);
                serviceResult = new ServiceResult(ServiceResultStatus.Success, dto, null, null);
            }

            return serviceResult;
        }

        public ServiceResult ResetPassword(ResetPasswordRequestDTO dto)
        {
            var user = dataContext.user.FirstOrDefault(u => u.reset_password_token == dto.token);
            if (user == null)
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Invalid verification token", 1003);
            }
            else
            {
                user.password = BCrypt.Net.BCrypt.HashPassword(dto.password);
                user.modified_at = DateTime.Now;
                user.reset_password_token = "";
                dataContext.SaveChanges();
                return new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
        }

        public ServiceResult UpdatePassword(UpdatePasswordDTO dto)
        {
            var userEmail = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            if (!BCrypt.Net.BCrypt.Verify(dto.oldPassword, user.password))
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Incorrect email or password", 1000);
            }
            else
            {
                user.password = BCrypt.Net.BCrypt.HashPassword(dto.newPassword);
                dataContext.SaveChanges();
                return new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
        }
        public ServiceResult UpdateUserInfo(UpdateUserInfoRequest request)
        {
            var userEmail = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            var path = Path.Combine(hostingEnvironment.ContentRootPath, "images/");

            //checking if "images" folder exist or not exist then create it
            if ((!Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
            //getting file name and combine with path and save it
            string filename = request.avatar.FileName;
            using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                request.avatar.CopyToAsync(fileStream);
            }
            user.display_name = request.displayName;
            user.full_name = request.fullName;
            DateTime birthday = DateTime.ParseExact(request.birthday, "yyyy-MM-dd", null);
            user.birthday = birthday;
            user.position = request.position;
            user.avatar = "images/" + filename;
            dataContext.SaveChanges();
            UpdateUserInfoResponseDTO response = new UpdateUserInfoResponseDTO(user.display_name, user.full_name, user.birthday.ToString(), user.position, user.avatar);
            return new ServiceResult(ServiceResultStatus.Success, response, null, null);
        }


    }





}


