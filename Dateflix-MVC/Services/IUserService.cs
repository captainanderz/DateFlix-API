using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IUserService
    {
        UserDto Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(int id);
        User GetByUsername(string username);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        IEnumerable<Claim> GetUserClaims(User user);
    }
}
