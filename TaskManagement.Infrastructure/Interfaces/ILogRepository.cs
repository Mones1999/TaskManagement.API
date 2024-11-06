using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using Task = System.Threading.Tasks.Task;
namespace TaskManagement.Infrastructure.Interfaces
{
    public interface ILogRepository
    {
        Task LogActionAsync(int userId, string action, DateTime time, string description, string username);
        Task<IEnumerable<ActivityLog>> GetLogsAsync();
        Task<IEnumerable<ActivityLog>> GetFilteredActivityLogs(int? userId, string action, DateTime? dateFrom, DateTime? dateTo);
    }
}
