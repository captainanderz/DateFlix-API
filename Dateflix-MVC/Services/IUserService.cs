using System.Collections.Generic;
using System.Security.Claims;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByUsername(string username);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        IEnumerable<Claim> GetUserClaims(User user);
    }
}