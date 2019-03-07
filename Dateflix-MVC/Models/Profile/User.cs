using System;
using System.Collections.Generic;
using DateflixMVC.Dtos;

namespace DateflixMVC.Models.Profile
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public List<RolerUser> Roles { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime Birthday { get; set; }
        public string[] ProfilePictures { get; set; }
        public string City { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public List<Inquiries> Inquiries { get; set; }
        public List<Blocks> Blocks { get; set; }
        public List<Likes> Likes { get; set; }
        public List<DirectMessages> DirectMessages { get; set; }
    }
}
