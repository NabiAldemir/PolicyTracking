using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DataAccessLayer.SignalR
{
    public class UserStatusHub : Hub
    {
        private static readonly Dictionary<string, string> ConnectedUsers = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[Context.ConnectionId] = userId;
                var onlineUsers = ConnectedUsers.Values.Distinct().ToList();
                Clients.All.SendAsync("UpdateUserList", onlineUsers);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.Remove(Context.ConnectionId);
            var onlineUsers = ConnectedUsers.Values.Distinct().ToList();
            Clients.All.SendAsync("UpdateUserList", onlineUsers);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
