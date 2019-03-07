using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Models.Profile
{
    public class DirectMessages
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
