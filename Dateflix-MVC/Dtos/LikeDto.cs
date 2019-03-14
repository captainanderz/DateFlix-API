using System;

namespace DateflixMVC.Dtos
{
    public class LikeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LikedId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
