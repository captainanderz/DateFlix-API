using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Models.Profile
{
    public class Match
    {
        public int Id { get; set; }
        public int UserOneId { get; set; }
        public int UserTwoId { get; set; }
    }
}
