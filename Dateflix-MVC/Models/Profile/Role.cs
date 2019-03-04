using System.Collections.Generic;

namespace DateflixMVC.Models.Profile
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RolerUser> Users { get; set; }
    }
}
