using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using Task = System.Threading.Tasks.Task;
namespace TaskManagement.Infrastructure.Interfaces
{
    public interface INotificationRepository
    {
        Task SaveNotificationAsync(Notification notification);
        Task<IEnumerable<int>> GetAllAdminsIds();
        Task<IEnumerable<Notification>> GetUserNotificationAsync(int userId);
        Task MarkAsRead(int notificationId);
        Task<int> GenerateId();
    }
}
