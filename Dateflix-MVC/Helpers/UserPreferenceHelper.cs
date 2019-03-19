using System;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Helpers
{
    public static class UserPreferenceHelper
    {
        public static bool IsValid(int minimum, int maximum)
        {
            return minimum.CompareTo(maximum) <= 0;
        }

        public static bool AgeRangeContainsValue(int value, UserPreference userPreference)
        {
            return userPreference.MinimumAge.CompareTo(value) <= 0 && (value.CompareTo(userPreference.MaximumAge) <= 0);
        }

        public static bool IsInsideRange(int value, UserPreference userPreference)
        {
            return IsValid(userPreference.MinimumAge, userPreference.MaximumAge) && AgeRangeContainsValue(value, userPreference);
        }
    }
}
