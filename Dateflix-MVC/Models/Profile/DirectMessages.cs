using System;

namespace DateflixMVC.Models.Profile
{
    public class DirectMessages
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
