using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using EntityTask = TaskManagement.Core.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly INotificationService _notificationService;
        public TaskService(ITaskRepository taskRepository, INotificationService notificationService)
        {
            _taskRepository = taskRepository;
            _notificationService = notificationService;
        }

        
        public async Task AddTaskAsync(EntityTask task)
        {
            await _taskRepository.AddTaskAsync(task);
            
        }

        
        public async Task<EntityTask> GetTaskByIdAsync(int taskId)
        {
            return await _taskRepository.GetTaskByIdAsync(taskId);
        }

        
        public async Task<IEnumerable<EntityTask>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }


        public async Task<IEnumerable<EntityTask>> GetTasksByUserIdAsync(int userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }


        public async Task UpdateTaskAsync(EntityTask task)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(task.TaskId);
            if (existingTask == null) 
            {
                throw new InvalidOperationException("Task Not Found!");
            }
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;
            existingTask.AssignedUserId = task.AssignedUserId;

            await _taskRepository.UpdateTaskAsync(existingTask);
        }


        public async Task DeleteTaskAsync(int taskId)
        {
            await _taskRepository.DeleteTaskAsync(taskId);
        }


        public async Task<IEnumerable<Comment>> GetTaskCommentsAsync(int taskId)
        {
            return await _taskRepository.GetTaskCommentsAsync(taskId);
        }


        public async Task AddCommentToTaskAsync(Comment comment)
        {
            
            await _taskRepository.AddCommentToTaskAsync(comment);
        }

        public async Task<IEnumerable<EntityTask>> GetTasksByStatusAsync(string status)
        {
            return await _taskRepository.GetTasksByStatusAsync(status);
        }

        public async Task<string> getUserNameForComment(int commentId)
        {
            var userName = await _taskRepository.getUserNameForComment(commentId);
            if (string.IsNullOrEmpty(userName)) 
            {
                throw new InvalidOperationException("User Not Found");
            }
            return userName;
        }
    }
}
