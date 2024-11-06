using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;

namespace TaskManagement.API.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> getLogs() 
        {
            try
            {
                var logs = await _logService.GetLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("GetFilteredActivityLogs")]
        public async Task<IActionResult> GetFilteredActivityLogs(int? userId,string? action,DateTime? dateFrom,DateTime? dateTo)
        {
            try
            {
                var logs = await _logService.GetFilteredActivityLogs(userId, action, dateFrom, dateTo);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
