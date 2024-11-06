using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.DTOs;
using TaskManagement.Infrastructure.Interfaces;

namespace TaskManagement.Application.Services
{
    public class EditProfileService : IEditProfileService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginRepository _userLoginRepository;

        public EditProfileService(IUserRepository userRepository, IUserLoginRepository userLoginRepository)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
        }

        public async Task EditProfileAsync(UserWithLoginDTO userWithLoginDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userWithLoginDto.UserId);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            var existingUserLogin = await _userLoginRepository.GetUserLoginByIdAsync(userWithLoginDto.UserLoginId);
            if (existingUserLogin == null)
            {
                throw new InvalidOperationException("User login information does not exist.");
            }

            var EmailIsUsed = await IsEmailUsed(userWithLoginDto.UserId, userWithLoginDto.Email);
            if (EmailIsUsed)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var UsernameIsUsed = await IsUsernameUsed(userWithLoginDto.UserId, userWithLoginDto.Username);
            if (UsernameIsUsed)
            {
                throw new InvalidOperationException("A user with this username already exists.");
            }

            existingUserLogin.Username = userWithLoginDto.Username;
            if (!string.IsNullOrEmpty(userWithLoginDto.Password))
            {
                existingUserLogin.PasswordHash = HashPassword(userWithLoginDto.Password);
            }
            existingUserLogin.Role = userWithLoginDto.Role;


            existingUser.Email = userWithLoginDto.Email;
            existingUser.FullName = userWithLoginDto.FullName;
            existingUser.Status = userWithLoginDto.Status;


            await _userLoginRepository.UpdateUserLoginAsync(existingUserLogin);
            await _userRepository.UpdateUserAsync(existingUser);
        }

        private string HashPassword(string password)
        {
            return password;
        }

        public async Task<bool> IsEmailUsed(int userId, string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return false;

            if (user.Email.Equals(email) && user.UserId != userId)
                return true;

            return false;
        }

        public async Task<bool> IsUsernameUsed(int userId, string username)
        {
            var userLogin = await _userLoginRepository.GetUserLoginByUsernameAsync(username);

            if (userLogin == null)
                return false;

            var user = await _userRepository.GetUserByUserLoginId(userLogin.UserLoginId);

            if (userLogin.Username.Equals(username) && user.UserId != userId)
                return true;

            return false;
        }

        public async Task<UserWithLoginDTO> GetUserInformationByIdAsync(int userId)
        {
            
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            var userLogin = await _userLoginRepository.GetUserLoginByIdAsync(user.UserLoginId);
            if (userLogin == null)
            {
                throw new InvalidOperationException("User login information does not exist.");
            }

            return new UserWithLoginDTO
            {
                UserId = user.UserId,
                UserLoginId = user.UserLoginId,
                Username = userLogin.Username,
                Role = userLogin.Role,
                Email = user.Email,
                FullName = user.FullName,
                Status = user.Status
            };
        }
    }
}
