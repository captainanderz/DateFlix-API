using System.ComponentModel.DataAnnotations.Schema;

namespace DateflixMVC.Models.Profile
{
    public class RolerUser
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
