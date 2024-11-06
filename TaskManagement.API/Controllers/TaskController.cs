using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;
using EntityTask = TaskManagement.Core.Entities.Task;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;

        public TaskController(ITaskService taskService, INotificationService notificationService)
        {
            _taskService = taskService;
            _notificationService = notificationService;
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(EntityTask task)
        {
            try
            {
               await _notificationService.NotifyUser(task.AssignedUserId.ToString(), $"A new task ({task.Title}) Assigned to you");
                if (task == null)
                    return BadRequest(new { message = "Task is null." });
                await _taskService.AddTaskAsync(task);
                return Ok("Task Created Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(taskId);
                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByUserIdAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(EntityTask task)
        {
            try
            {
                var OldTask = await _taskService.GetTaskByIdAsync(task.TaskId);
                if (task.AssignedUserId != OldTask.AssignedUserId)
                {
                    await _notificationService.NotifyUser(task.AssignedUserId.ToString(), $"Task ({task.Title}) assigned to you");
                }
                else
                {
                    await _notificationService.NotifyUser(task.AssignedUserId.ToString(), $"An Admin updated your task ({task.Title})");
                }

                await _taskService.UpdateTaskAsync(task);
                return Ok("Task Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(taskId);
                if (task == null)
                    return NotFound();

                await _taskService.DeleteTaskAsync(taskId);
                return Ok("Task Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{taskId}/comments")]
        public async Task<IActionResult> GetTaskComments(int taskId)
        {
            try
            {
                var comments = await _taskService.GetTaskCommentsAsync(taskId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddCommentToTask(int taskId, Comment comment)
        {
            try
            {
                if (comment == null)
                    return BadRequest(new { message = "Comment is null." });

                comment.TaskId = taskId;
                await _taskService.AddCommentToTaskAsync(comment);

                var task = await _taskService.GetTaskByIdAsync(comment.TaskId);
                if (task != null) 
                {
                    await _notificationService.NotifyUser(task.AssignedUserId.ToString(), $"An admin commented on your task ({task.Title})");
                }
                return Ok("Comment Added Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTasksByStatus(string status)
        {
            try
            {
                var tasks = await _taskService.GetTasksByStatusAsync(status);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("getUserNameForComment/{commentId}")]
        public async Task<IActionResult> getUserNameForComment(int commentId) 
        {
            try
            {
                string userName = await _taskService.getUserNameForComment(commentId);
                return Ok(userName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
