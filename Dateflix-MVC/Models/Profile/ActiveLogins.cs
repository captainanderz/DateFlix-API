namespace DateflixMVC.Models.Profile
{
    public class ActiveLogins
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JwtToken { get; set; }
    }
}
