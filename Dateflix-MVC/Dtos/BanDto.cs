using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Dtos
{
    public class BanDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
