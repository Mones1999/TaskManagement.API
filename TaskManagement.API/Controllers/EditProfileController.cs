using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Constants;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.DTOs;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditProfileController : ControllerBase
    {
        private readonly IEditProfileService _editProfileService;
        private readonly INotificationService _notificationService;
        private readonly ILogService _logService;
        public EditProfileController(IEditProfileService editProfileService, INotificationService notificationService, ILogService logService)
        {
            _editProfileService = editProfileService;
            _notificationService = notificationService;
            _logService = logService;
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId) 
        {
            try
            {
                var user = await _editProfileService.GetUserInformationByIdAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile(UserWithLoginDTO userWithLoginDto)
        {
            try
            {
                await _editProfileService.EditProfileAsync(userWithLoginDto);
                await _logService.LogActionAsync(LogConstants.Update, $"User ({userWithLoginDto.Username}) Update his profile");
                await _notificationService.NotifyAdmins($"User ({userWithLoginDto.Username}) Update his profile");
                return Ok("Your profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("IsEmailUsed")]
        public async Task<IActionResult> IsEmailUsed(string email, int userId = 0)
        {
            try
            {
                bool isInUse = await _editProfileService.IsEmailUsed(userId, email);
                return Ok(isInUse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("IsUsernameUsed")]
        public async Task<IActionResult> IsUsernameUsed(string username, int userId = 0)
        {
            try
            {
                bool isInUse = await _editProfileService.IsUsernameUsed(userId, username);
                return Ok(isInUse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
