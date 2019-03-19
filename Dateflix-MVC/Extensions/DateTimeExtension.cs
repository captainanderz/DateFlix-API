using System;

namespace DateflixMVC.Extensions
{
    public static class DateTimeExtension
    {
        public static int ToAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            {
                age = age - 1;
            }

            return age;
        }
    }
}
