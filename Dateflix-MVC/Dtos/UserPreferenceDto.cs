using DateflixMVC.Models.Profile;

namespace DateflixMVC.Dtos
{
    public class UserPreferenceDto
    {
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public Gender Gender { get; set; }
    }
}
