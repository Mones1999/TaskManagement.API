using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using Task = System.Threading.Tasks.Task;
using System.Security.Cryptography;

namespace TaskManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginRepository _userLoginRepository;

        public UserService(IUserRepository userRepository, IUserLoginRepository userLoginRepository)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
        }
        public async Task CreateUserWithLoginAsync(UserWithLoginDTO userWithLoginDto)
        {
            
            var existingUserByEmail = await _userRepository.GetUserByEmailAsync(userWithLoginDto.Email);
            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }
            
            var existingUserLoginByUsername = await _userLoginRepository.GetUserLoginByUsernameAsync(userWithLoginDto.Username);
            if (existingUserLoginByUsername != null)
            {
                throw new InvalidOperationException("A user with this username already exists.");
            }

            var userLogin = new UserLogin
            {
                Username = userWithLoginDto.Username,
                PasswordHash = HashPassword(userWithLoginDto.Password), 
                Role = userWithLoginDto.Role
            };

            await _userLoginRepository.AddUserLoginAsync(userLogin);

            
            var user = new User
            {
                UserLoginId = userLogin.UserLoginId, 
                Email = userWithLoginDto.Email,
                FullName = userWithLoginDto.FullName,
                Status = userWithLoginDto.Status
            };

            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserWithLoginAsync(UserWithLoginDTO userWithLoginDto)
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

            var EmailIsUsed = await IsEmailInUseByAnotherUserAsync(userWithLoginDto.Email, userWithLoginDto.UserId);
            if (EmailIsUsed)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var UsernameIsUsed = await IsUsernameUseByAnotherUserAsync(userWithLoginDto.Username, userWithLoginDto.UserId);
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

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            await _userLoginRepository.ChangePassword(user.UserLogin.UserLoginId, HashPassword(newPassword));
        }

        public async Task<UserWithLoginDTO> GetUserWithLoginByIdAsync(int userId)
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

        public async Task DeleteUserWithLoginAsync(int userId)
        {
            // Fetch the user and associated login info
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

            // Delete both User and UserLogin
            await _userRepository.DeleteUserAsync(userId);
            await _userLoginRepository.DeleteUserLoginAsync(user.UserLoginId);
        }

        public async Task<IEnumerable<UserWithLoginDTO>> GetAllUsersWithLoginAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userLogins = await _userLoginRepository.GetAllUserLoginsAsync();

            return users.Select(user => {
                var userLogin = userLogins.FirstOrDefault(ul => ul.UserLoginId == user.UserLoginId);
                return new UserWithLoginDTO
                {
                    UserId = user.UserId,
                    UserLoginId = user.UserLoginId,
                    Username = userLogin?.Username ?? "",
                    Role = userLogin?.Role ?? "",
                    Email = user.Email,
                    FullName = user.FullName,
                    Status = user.Status
                };
            }).ToList();
        }
        
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("PinCode cannot be null or empty.");

            using (var sha256 = SHA256.Create())
            {
                var hash = new StringBuilder();
                byte[] crypto = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                foreach (byte theByte in crypto)
                {
                    hash.Append(theByte.ToString("x2"));
                }
                return hash.ToString();
            }
        }

        public async Task<bool> IsEmailInUseByAnotherUserAsync(string email, int userId = 0)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if(user == null) 
                return false;

            if (user.Email.Equals(email) && user.UserId != userId) 
                return true;
            
            return false;
        }

        public async Task<bool> IsUsernameUseByAnotherUserAsync(string username, int userId = 0)
        {
            var userLogin = await _userLoginRepository.GetUserLoginByUsernameAsync(username);
            
            if (userLogin == null) 
                return false;

            var user = await _userRepository.GetUserByUserLoginId(userLogin.UserLoginId);

            if (userLogin.Username.Equals(username) && user.UserId != userId)
                return true;

            return false;
        }
    }
}
