using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Core.DTOs;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces
{
    public interface IEditProfileService
    {
        Task<UserWithLoginDTO> GetUserInformationByIdAsync(int userId);
        Task EditProfileAsync(UserWithLoginDTO userWithLoginDTO);
        Task<bool> IsEmailUsed(int userId, string email);
        Task<bool> IsUsernameUsed(int userId, string username);
    }
}
