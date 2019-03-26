using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Extensions;
using DateflixMVC.Models.SignalR;
using DateflixMVC.Services;
using Microsoft.AspNetCore.SignalR;

namespace DateflixMVC.Hubs
{
    public class PrivateChatHub : Hub
    {
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();

        private IMessageService _messageService;

        public PrivateChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Called when a new connection is connected to the hub
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var email = httpContext.Request.Query["email"].ToString();

            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            ConnectedUsers.AddUser(email, Context.ConnectionId);

            await base.OnConnectedAsync();
            await Clients.Client(Context.ConnectionId).SendCoreAsync("SetConnectionId", new object[] { Context.ConnectionId });
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            ConnectedUsers.RemoveUser(connectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message, string email, string senderConnectionId, string receiverConnectionId, string receiverEmail = null)
        {
            if (string.IsNullOrWhiteSpace(receiverConnectionId))
            {
                receiverConnectionId = GetConnectionIdFromEmail(receiverEmail);
            }

            if (receiverConnectionId != null)
            {
                await Clients.Clients(receiverConnectionId).SendCoreAsync("ReceiveMessage", new object[] { email, message, senderConnectionId });
            }

            // Save message to DB
            await _messageService.SaveMessage(email, receiverEmail, message);
        }

        public List<MessageDto> GetMessages(string senderEmail, string senderConnectionId, string receiverEmail)
        {
            var messages = _messageService.GetMessages(senderEmail, receiverEmail);
            return messages;
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
