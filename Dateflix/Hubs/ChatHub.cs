using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Dateflix.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendCoreAsync("ReceiveMessage", new object[] {username, message});
        }
    }

    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
