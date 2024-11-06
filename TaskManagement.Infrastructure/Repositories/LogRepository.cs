using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using TaskManagement.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly AppDbContext _context;
        public LogRepository(AppDbContext context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<ActivityLog>> GetLogsAsync()
        {
            return await _context.ActivityLogs.OrderByDescending(log => log.Timestamp).ToListAsync();
        }

        public async Task LogActionAsync(int userId, string action, DateTime time, string description, string username)
        {
            var log = new ActivityLog
            { 
                Action = action,
                UserId = userId,
                Timestamp = time,
                Description = description,
                Username = username
            };
            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ActivityLog>> GetFilteredActivityLogs(int? userId, string action, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.ActivityLogs.AsQueryable();

            // Apply userId filter
            if (userId.HasValue)
                query = query.Where(log => log.UserId == userId.Value);

            
            if (!string.IsNullOrEmpty(action))
                query = query.Where(log => log.Action.Contains(action));

            
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(log => log.Timestamp >= dateFrom.Value && log.Timestamp <= dateTo.Value);
            }
            else if (dateFrom.HasValue)
            {
                query = query.Where(log => log.Timestamp >= dateFrom.Value);
            }
            else if (dateTo.HasValue)
            {
                query = query.Where(log => log.Timestamp <= dateTo.Value);
            }

            return await query.OrderByDescending(log=> log.Timestamp).ToListAsync();
        }

    }
}
