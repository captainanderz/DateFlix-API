using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateflixMVC.Extensions;
using DateflixMVC.Models.SignalR;
using DateflixMVC.Services;
using Microsoft.AspNetCore.SignalR;

namespace DateflixMVC.Hubs
{
    public class PrivateChatHub : Hub
    {
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();

        private IUserService _userService;
        private IMessageService _messageService;

        public PrivateChatHub(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync() // Called when a new connection is connected to the hub
        {
            var httpContext = Context.GetHttpContext();
            var email = httpContext.Request.Query["email"].ToString();

            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            ConnectedUsers.AddConnectionIdToUser(email, Context.ConnectionId);

            await base.OnConnectedAsync();
            await Clients.Client(Context.ConnectionId).SendCoreAsync("SetConnectionId", new object[] { Context.ConnectionId });
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            ConnectedUsers.RemoveUser(connectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message, string username, string senderConnectionId, string receiverConnectionId, string receiverUsername = null)
        {
            if (string.IsNullOrWhiteSpace(receiverConnectionId))
            {
                receiverConnectionId = GetConnectionIdFromEmail(receiverUsername);
            }

            if (receiverConnectionId != null)
            {
                await Clients.Clients(receiverConnectionId).SendCoreAsync("ReceiveMessage", new object[] { username, message, senderConnectionId });
            }

            // Save message to DB
            await _messageService.SaveMessage(username, receiverUsername, message);
        }

        public async Task GetMessages(string senderUsername, string senderConnectionId, string receiverUsername)
        {
            var messages = _messageService.GetMessages(senderUsername, receiverUsername);
            await Clients.Clients(senderConnectionId).SendCoreAsync("GetMessages", new object[] {messages});
        }

        public void AddUserList(string username, string connectionId)
        {
            ConnectedUsers.Add(new UserDetail()
            {
                ConnectionId = connectionId,
                Email = username
            });

            //await Clients.All.SendCoreAsync("SetList", new object[] {ConnectedUsers});
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public string GetConnectionIdFromEmail(string email, string senderConnectionId = null)
        {
            var user = ConnectedUsers.FirstOrDefault(x => x.Email == email);
            if (user?.Email != null && senderConnectionId != null)
            {
                Clients.Clients(senderConnectionId)
                    .SendCoreAsync("ReceiveConnectionIdFromEmail", new object[] { user.ConnectionId });
            }

            return user?.ConnectionId;
        }
    }
}
