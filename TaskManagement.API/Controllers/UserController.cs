
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.Application.Constants;

namespace TaskManagement.API.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;

        public UserController(IUserService userService, ILogService logService, INotificationService notificationService)
        {
            _userService = userService;
            _logService = logService;
            _notificationService = notificationService;
        }

        [HttpPost("CreateUserWithLogin")]
        public async Task<IActionResult> CreateUserWithLogin(UserWithLoginDTO userWithLoginDto)
        {
            try
            {
                await _userService.CreateUserWithLoginAsync(userWithLoginDto);

                await _logService.LogActionAsync(LogConstants.Create, $"Admin Add User ({userWithLoginDto.Username})");
                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPut("UpdateUserWithLogin")]
        public async Task<IActionResult> UpdateUserWithLogin(UserWithLoginDTO userWithLoginDto)
        {
            try
            {
                await _notificationService.NotifyUser(userWithLoginDto.UserId.ToString(), "An Admin updated your profile");

                await _userService.UpdateUserWithLoginAsync(userWithLoginDto);

                await _logService.LogActionAsync(LogConstants.Update, $"Admin Update User ({userWithLoginDto.Username})");
                
                
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                await _notificationService.NotifyUser(model.UserId.ToString(), $"An Admin updated your password");
                await _userService.ChangePasswordAsync(model.UserId, model.NewPassword);
                await _logService.LogActionAsync(LogConstants.Update, $"Admin Update password for User (user id: {model.UserId})");
                
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetUserWithLogin/{id}")]
        public async Task<IActionResult> GetUserWithLogin(int id)
        {
            try
            {
                var userWithLogin = await _userService.GetUserWithLoginByIdAsync(id);
                return Ok(userWithLogin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllUsersWithLogin")]
        public async Task<IActionResult> GetAllUsersWithLogin()
        {
            try
            {
                var users = await _userService.GetAllUsersWithLoginAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("DeleteUserWithLogin/{id}")]
        
        public async Task<IActionResult> DeleteUserWithLogin(int id)
        {
            try
            {
                await _userService.DeleteUserWithLoginAsync(id);
                await _logService.LogActionAsync(LogConstants.Delete, $"Admin Deleted User (user id: {id})");

                return Ok("User and UserLogin deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("IsEmailInUseByAnotherUser")]
        public async Task<IActionResult> IsEmailInUseByAnotherUser(string email, int userId = 0)
        {
            try
            {
                bool isInUse = await _userService.IsEmailInUseByAnotherUserAsync(email, userId);
                return Ok(isInUse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("IsUsernameInUseByAnotherUser")]
        public async Task<IActionResult> IsUsernameInUseByAnotherUser(string username, int userId = 0)
        {
            try
            {
                bool isInUse = await _userService.IsUsernameUseByAnotherUserAsync(username, userId);
                return Ok(isInUse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
