using System;

namespace DateflixMVC.Dtos
{
    public class BlockDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BlockedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
