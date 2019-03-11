using System;

namespace DateflixMVC.Models.Profile
{
    public class Inquiries
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
