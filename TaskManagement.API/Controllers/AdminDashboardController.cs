using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        
        [HttpGet("GetNumberOfUsers")]
        public async Task<IActionResult> GetNumberOfUsers()
        {
            try
            {
                var totalUsers = await _dashboardService.getNumberOfUsers();
                return Ok(totalUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetNumberOfTasks")]
        public async Task<IActionResult> GetNumberOfTasks()
        {
            try
            {
                var totalTasks = await _dashboardService.getNumberOfTasks();
                return Ok(totalTasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetNumberOfCompletedTasks")]
        public async Task<IActionResult> GetNumberOfCompletedTasks()
        {
            try
            {
                var completedTasks = await _dashboardService.getNumberOfCompletedTasks();
                return Ok(completedTasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getNumberOfPendingTasks")]
        public async Task<IActionResult> getNumberOfPendingTasks()
        {
            try
            {
                var inProgressTasks = await _dashboardService.getNumberOfPendingTasks();
                return Ok(inProgressTasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetUsersByTopNumberOfTasks")]
        public async Task<IActionResult> GetUsersByTopNumberOfTasks(int numberOfUser = 5)
        {
            try
            {
                var topUsers = await _dashboardService.getUsersByTopNumberOfTasks(numberOfUser);
                return Ok(topUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
