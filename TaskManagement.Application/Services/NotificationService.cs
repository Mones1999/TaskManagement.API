using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Hubs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepository)
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }

        public async Task NotifyUser(string userId, string message)
        {
            var newId = await _notificationRepository.GenerateId();
            var notification = new Notification {
                NotificationId = newId,
                UserId = Convert.ToInt32(userId),
                Message = message,
                CreatedDate = DateTime.Now,
                IsRead = false,
            };

            await _notificationRepository.SaveNotificationAsync(notification);

            var connectionId = NotificationHub.GetConnectionIdByUserId(userId);
            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
            }
            
        }

        public async Task NotifyAdmins(string message)
        {
            var AdminIds = await _notificationRepository.GetAllAdminsIds();
            foreach (var adminId in AdminIds) 
            { 
                await NotifyUser(adminId.ToString(), message);
            }
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationAsync(int userId)
        {
            return await _notificationRepository.GetUserNotificationAsync(userId);
        }

        public async Task MarkAsRead(int notificationId)
        {
            await _notificationRepository.MarkAsRead(notificationId);
        }
    }
}
