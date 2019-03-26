using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IUserService
    {
        UserDto Authenticate(string email, string password);
        List<User> GetAll();
        Task<User> GetByIdAsync(int id);
        User GetByEmail(string email);
        User Create(User user, string password);
        void Update(User user, string password = null);
        bool UpdateUserPreference(int id, UserPreference userPreference);
        void Delete(int id);
        IEnumerable<Claim> GetUserClaims(User user);
        User GetById(int id);
        bool AgeRangeContainsValue(int value, UserPreference userPreference);
        bool IsAgeInsideRange(int value, UserPreference userPreference);
    }
}
