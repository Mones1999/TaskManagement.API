using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.Entities;
using TaskManagement.Infrastructure.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogService(ILogRepository logRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<ActivityLog>> GetLogsAsync()
        {
            return await _logRepository.GetLogsAsync();
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string? token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                return new ClaimsPrincipal(claimsIdentity);
            }
            else
            {
                return _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
            }
        }
        private int getUserIdFromToken(string? token = null)
        {
            var claimsPrincipal = GetClaimsPrincipalFromToken(token);
            var userIdClaim = claimsPrincipal.FindFirst("userId")?.Value;
            return userIdClaim != null ? Convert.ToInt32(userIdClaim) : 0; // Default to 0 or handle as needed if userId is null
        }

        private string getUserNameFromToken(string? token = null)
        {
            var claimsPrincipal = GetClaimsPrincipalFromToken(token);
            return claimsPrincipal.FindFirst("userName")?.Value ?? string.Empty;
        }

        public Task LogActionAsync(string action, string description, string? token)
        {
            var userId = getUserIdFromToken(token);
            var username = getUserNameFromToken(token);
            return _logRepository.LogActionAsync(userId, action, DateTime.Now, description, username);
        }

        public async Task<IEnumerable<ActivityLog>> GetFilteredActivityLogs(int? userId, string action, DateTime? dateFrom, DateTime? dateTo)
        {
            return await _logRepository.GetFilteredActivityLogs(userId, action, dateFrom, dateTo);
        }
    }
}
