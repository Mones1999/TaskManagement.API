using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using TaskManagement.Infrastructure.Data;
using EntityTask = TaskManagement.Core.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {

        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task AddTaskAsync(EntityTask task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        
        public async Task<EntityTask> GetTaskByIdAsync(int taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
            
        }

        
        public async Task<IEnumerable<EntityTask>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        
        public async Task<IEnumerable<EntityTask>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks.Where(t => t.AssignedUserId == userId).ToListAsync();
        }

        
        public async Task UpdateTaskAsync(EntityTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            var task = await GetTaskByIdAsync(taskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task<IEnumerable<Comment>> GetTaskCommentsAsync(int taskId)
        {
            return await _context.Comments.Where(c => c.TaskId == taskId).OrderByDescending(c=> c.CommentId).ToListAsync();
        }

        
        public async Task AddCommentToTaskAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        
        public async Task<IEnumerable<EntityTask>> GetTasksByStatusAsync(string status)
        {
            return await _context.Tasks.Where(t => t.Status == status).ToListAsync();
        }

        public async Task<string> getUserNameForComment(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
            if (comment == null) return null;
            var userId = comment.UserId;
            var user = await _context.Users.Include(u => u.UserLogin).FirstOrDefaultAsync(u => u.UserId == userId);
            return user.UserLogin.Username;
        }

        public async Task<int> getNumberOfCompletedTasks()
        {
            return await _context.Tasks.Where(t=> t.Status.Equals("Completed")).CountAsync();
        }

        public async Task<int> getNumberOfPendingTasks()
        {
            return await _context.Tasks.Where(t => t.Status.Equals("Pending")).CountAsync();
        }

        public async Task<int> getNumberOfTasks() 
        { 
            return await _context.Tasks.CountAsync();
        }
    }
}

