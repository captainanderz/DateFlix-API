﻿using System;

namespace DateflixMVC.Models.Profile
{
    public class Likes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LikedId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
