using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Chat.Hubs
{
    [Route("/chatHub")]
    public class ChatHub : Hub
    {
        public Task SendMessageToAll(string message)
        {
            var res = Clients.All.SendAsync("ReceiveMessage", message);
            return res;
        }
        public Task SendMessageToCaller(string message)
        {
            var res = Clients.Caller.SendAsync("ReceiveMessage", message);
            return res;
        }
        public Task SendMessageToUser(string connectionId, string message)
        {
            var res = Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            return res;
        }
        public Task JoinGroup(string group)
        {
            var res = Groups.AddToGroupAsync(Context.ConnectionId, group);
            return res;
        }
        public Task LeaveGroup(string group)
        {
            var res = Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
            return res;
        }
        public Task SendMessageToGroup(string group, string message)
        {
            var res = Clients.Group(group).SendAsync("ReceiveMessage", message);
            return res;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
