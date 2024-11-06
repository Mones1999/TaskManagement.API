using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using EntityTask = TaskManagement.Core.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task AddTaskAsync(EntityTask task);
        Task<EntityTask> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<EntityTask>> GetAllTasksAsync();
        Task<IEnumerable<EntityTask>> GetTasksByUserIdAsync(int userId);
        Task UpdateTaskAsync(EntityTask task);
        Task DeleteTaskAsync(int taskId);
        Task<IEnumerable<Comment>> GetTaskCommentsAsync(int taskId);
        Task AddCommentToTaskAsync(Comment comment);
        Task<IEnumerable<EntityTask>> GetTasksByStatusAsync(string status);
        Task<string> getUserNameForComment(int commentId);
    }
}
