using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Interfaces
{
    public interface IUserLoginRepository
    {
        Task<UserLogin> GetUserLoginByIdAsync(int userLoginId);
        Task<UserLogin> GetUserLoginByUsernameAsync(string username);
        Task<IEnumerable<UserLogin>> GetAllUserLoginsAsync();
        Task AddUserLoginAsync(UserLogin userLogin);
        Task UpdateUserLoginAsync(UserLogin userLogin);
        Task ChangePassword(int userLoginId, string newPassword);
        Task DeleteUserLoginAsync(int userLoginId);
        
    }
}
