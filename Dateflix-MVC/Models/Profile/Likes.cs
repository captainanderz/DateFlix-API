using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
