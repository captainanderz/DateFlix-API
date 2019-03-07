using System;

namespace DateflixMVC.Models
{
    public class Blocks
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BlockedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
