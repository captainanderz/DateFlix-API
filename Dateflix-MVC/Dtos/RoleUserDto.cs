using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Dtos
{
    public class RoleUserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
    }
}
