using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateflixMVC.Models.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace DateflixMVC.Hubs
{
    public class PrivateChatHub : Hub
    {
        #region Data Members
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        public static List<MessageDetail> CurrentMessage = new List<MessageDetail>();
        #endregion

        public override Task OnConnectedAsync() // Called when a new connection is connected to the hub
        {
            return Clients.Client(Context.ConnectionId).SendCoreAsync("SetConnectionId", new object[] {Context.ConnectionId});
        }

        public Task AddUserList(string userName, string connectionId)
        {
            ConnectedUsers.Add(new UserDetail()
            {
                ConnectionId = connectionId,
                UserName = userName
            });

            return Clients.All.SendCoreAsync("SetList", new object[] {ConnectedUsers});
        }
    }
}
