using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Application.Constants;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;
        private readonly ILogService _logService;
        public UserTasksController(ITaskService taskService, INotificationService notificationService, ILogService logService)
        {
            _taskService = taskService;
            _notificationService = notificationService;
            _logService = logService;
        }

        [HttpGet("GetTaskById/{taskId}")]
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

        [HttpGet("GetTasksByUserId/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByUserIdAsync(userId);
                if (tasks == null)
                {
                    return NotFound($"No tasks found for user with ID {userId}");
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpPut("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, string newStatus)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(taskId);
                if (task == null)
                {
                    return NotFound($"Task with ID {taskId} not found");
                }

                task.Status = newStatus;
                await _taskService.UpdateTaskAsync(task);

                await _notificationService.NotifyAdmins($"User Updated task status to ({newStatus}) for task ({task.Title})");
                await _logService.LogActionAsync(LogConstants.Update, $"User Updated task status to ({newStatus}) for task ({task.Title})");
                
                return Ok("Task status updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpGet("GetTaskComments/{taskId}")]
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

        
        [HttpPost("AddCommentToTask")]
        public async Task<IActionResult> AddCommentToTask(int taskId,Comment comment)
        {
            try
            {
                if (comment == null)
                    return BadRequest(new { message = "Comment is null." });

                comment.TaskId = taskId;
                await _taskService.AddCommentToTaskAsync(comment);

                var task = await _taskService.GetTaskByIdAsync(taskId);

                await _notificationService.NotifyAdmins($"User Commented on task ({task.Title})");
                await _logService.LogActionAsync(LogConstants.Create, $"User Commented on task ({task.Title})");

                return Ok("Comment Added Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpGet("GetUserNameForComment/{commentId}")]
        public async Task<IActionResult> GetUserNameForComment(int commentId)
        {
            try
            {
                var username = await _taskService.getUserNameForComment(commentId);
                if (username == null)
                {
                    return NotFound($"User not found for comment with ID {commentId}");
                }
                return Ok(username);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
