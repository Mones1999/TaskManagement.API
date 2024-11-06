using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.DTOs;

namespace TaskManagement.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<int> getNumberOfUsers();
        Task<int> getNumberOfTasks();
        Task<int> getNumberOfCompletedTasks();
        Task<int> getNumberOfPendingTasks();
        Task<IEnumerable<UsersTaskDTO>> getUsersByTopNumberOfTasks(int numberOfUser);
        
    }
}
