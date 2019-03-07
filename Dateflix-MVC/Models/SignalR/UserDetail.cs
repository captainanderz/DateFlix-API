using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Models.SignalR
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
    }
}
