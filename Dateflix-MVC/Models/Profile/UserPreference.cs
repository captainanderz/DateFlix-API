namespace DateflixMVC.Models.Profile
{
    public class UserPreference
    {
        public int Id { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public Gender Gender { get; set; }
    }
}
