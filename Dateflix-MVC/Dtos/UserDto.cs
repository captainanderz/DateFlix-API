﻿using System;
using System.Collections.Generic;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<RoleUserDto> Roles { get; set; }
        public DateTime Birthday { get; set; }
        public string[] ProfilePictures { get; set; }
        public string City { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public List<Likes> Likes { get; set; }
        public UserPreference UserPreference { get; set; }
    }
}
