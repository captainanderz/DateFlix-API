using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RoleUserDto> Users { get; set; }
    }
}
