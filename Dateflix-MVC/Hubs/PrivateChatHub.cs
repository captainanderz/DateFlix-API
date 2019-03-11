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
        #region Data Members
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        public static List<MessageDetail> CurrentMessage = new List<MessageDetail>();
        #endregion

        private IUserService _userService;
        private IMessageService _messageService;

        public PrivateChatHub(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public override Task OnConnectedAsync() // Called when a new connection is connected to the hub
        {
            return Clients.Client(Context.ConnectionId).SendCoreAsync("SetConnectionId", new object[] { Context.ConnectionId });
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            ConnectedUsers.RemoveUser(connectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public void SendMessage(string username, string message, string receiverId, string senderId)
        {
            Clients.Clients(receiverId).SendCoreAsync("ReceiveMessage", new object[] { username, message, senderId });

            // Save message to DB
            _messageService.SaveMessage(senderId, receiverId, message);
        }

        public async Task AddUserList(string username, string connectionId)
        {
            ConnectedUsers.Add(new UserDetail()
            {
                ConnectionId = connectionId,
                Username = username
            });

            //await Clients.All.SendCoreAsync("SetList", new object[] {ConnectedUsers});
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public void GetConnectionIdFromUsername(string username, string senderConnectionId)
        {
            var user = ConnectedUsers.FirstOrDefault(x => x.Username == username);
            if (user?.Username != null)
            {
                Clients.Clients(senderConnectionId)
                    .SendCoreAsync("ReceiveConnectionIdFromUsername", new object[] { user.ConnectionId });
            }
        }
    }
}
