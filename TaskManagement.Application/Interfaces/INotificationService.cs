using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUser(string userId, string message);
        Task NotifyAdmins(string message);
        Task<IEnumerable<Notification>> GetUserNotificationAsync(int userId);
        Task MarkAsRead(int notificationId);
    }
}
