using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using System.Collections.Concurrent;

namespace TaskManagement.Application.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("userId")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("userId")?.Value;
            if (userId != null)
            {
                UserConnections.TryRemove(userId, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionIdByUserId(string userId)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                return connectionId;
            }
            else
            {
                return null;
            }
        }

    }
}
