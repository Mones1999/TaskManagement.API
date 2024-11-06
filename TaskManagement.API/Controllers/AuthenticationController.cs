using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;
using TaskManagement.Application.Constants;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.DTOs;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogService _logService;
        
        public AuthenticationController(IAuthenticationService authenticationService,ILogService logService)
        {
            _authenticationService = authenticationService;
            _logService = logService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCredentails loginDetails)
        {
            try
            {
                var token = await _authenticationService.Login(loginDetails);
                if (token == null)
                {
                    return Unauthorized();
                }
                await _logService.LogActionAsync(LogConstants.Login, "User Login", token);
                return new JsonResult(new { token = token });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _logService.LogActionAsync(LogConstants.Logout, "User Logout");
                return Ok("Logged out!");

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
