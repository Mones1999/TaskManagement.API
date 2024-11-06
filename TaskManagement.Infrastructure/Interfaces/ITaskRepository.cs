using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using EntityTask = TaskManagement.Core.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Interfaces
{
    public interface ITaskRepository
    {
        // Create a new task
        Task AddTaskAsync(EntityTask task);

        // Read operations
        Task<EntityTask> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<EntityTask>> GetAllTasksAsync();
        Task<IEnumerable<EntityTask>> GetTasksByUserIdAsync(int userId);

        // Update an existing task
        Task UpdateTaskAsync(EntityTask task);

        // Delete a task
        Task DeleteTaskAsync(int taskId);

        // Additional methods
        Task<IEnumerable<Comment>> GetTaskCommentsAsync(int taskId); // To retrieve comments for a task
        Task AddCommentToTaskAsync(Comment comment); // To add a comment to a task
        Task<IEnumerable<EntityTask>> GetTasksByStatusAsync(string status); // For filtering tasks by status

        Task<string> getUserNameForComment(int commentId);
        Task<int> getNumberOfCompletedTasks();
        Task<int> getNumberOfPendingTasks();
        Task<int> getNumberOfTasks();
    }
}
