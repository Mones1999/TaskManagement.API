using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;
using TaskManagement.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserWithLoginAsync(UserWithLoginDTO userWithLoginDto);
        Task UpdateUserWithLoginAsync(UserWithLoginDTO userWithLoginDto);
        Task ChangePasswordAsync(int userId, string newPassword);
        Task<UserWithLoginDTO> GetUserWithLoginByIdAsync(int userId);
        Task DeleteUserWithLoginAsync(int userId);
        Task<IEnumerable<UserWithLoginDTO>> GetAllUsersWithLoginAsync();
        Task<bool> IsEmailInUseByAnotherUserAsync(string email, int userId = 0);
        Task<bool> IsUsernameUseByAnotherUserAsync(string username, int userId = 0);
    }
}
