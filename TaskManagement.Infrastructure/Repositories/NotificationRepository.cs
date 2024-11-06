using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Interfaces;
using Task = System.Threading.Tasks.Task;


namespace TaskManagement.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> GetAllAdminsIds()
        {
            return await _context.UserLogins
                        .Where(userLogin => userLogin.Role == "admin")
                        .Select(userLogin => userLogin.User.UserId)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationAsync(int userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId).OrderByDescending(n=> n.CreatedDate).Take(7).ToListAsync();
        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            
            notification.IsRead = true;
            
            _context.Notifications.Update(notification);
            
            await _context.SaveChangesAsync();
        }

        public async Task SaveNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GenerateId() 
        {
            var notificationQuery =  _context.Notifications;
            if (notificationQuery.Count() > 0) 
            { 
                return await notificationQuery.MaxAsync(n=> n.NotificationId) + 1;
            }
            return 1;
        }


    }
}
