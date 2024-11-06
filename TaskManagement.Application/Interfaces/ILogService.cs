using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using Task = System.Threading.Tasks.Task;
namespace TaskManagement.Application.Interfaces
{
    public interface ILogService
    {
        Task LogActionAsync(string action, string description, string? token = null);
        Task<IEnumerable<ActivityLog>> GetLogsAsync();
        Task<IEnumerable<ActivityLog>> GetFilteredActivityLogs(int? userId, string action, DateTime? dateFrom, DateTime? dateTo);
    }
}
