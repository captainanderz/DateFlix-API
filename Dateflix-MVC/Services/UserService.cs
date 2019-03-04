using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using Microsoft.EntityFrameworkCore;

namespace DateflixMVC.Services
{
    public class UserService : IUserService
    {
        private WebApiDbContext _context;

        public UserService(WebApiDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.Include(x => x.Roles).ThenInclude(x => x.Role).SingleOrDefault(u => u.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // Authentication successful
            return user;
        }

        public User Create(User user, string password)
        {
            // Validation..
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username " + user.Username + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Include(x => x.Roles).ThenInclude(x => x.Role).Include(x => x.Roles).ThenInclude(x => x.User).SingleOrDefault(x => x.Id == id);
        }

        public void Update(User user, string password = null)
        {
            var userInDb = _context.Users.Find(user.Id);

            if (userInDb == null)
                throw new AppException("User not found");

            if (user.Username != userInDb.Username)
            {
                // username changed, checking for availability
                if (_context.Users.Any(x => x.Username == user.Username))
                    throw new AppException("Username " + user.Username + " is already taken");
            }

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.Username = user.Username;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userInDb.PasswordHash = passwordHash;
                userInDb.PasswordSalt = passwordSalt;
            }
            _context.Users.Update(userInDb);
            _context.SaveChanges();
        }

        // generates and returns an IEnumerable of claims including the all of the user's roles and their Id.
        public IEnumerable<Claim> GetUserClaims(User user)
        {
            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString())); // Add claim for user's id

            // Add claim for each of the users roles
            foreach (var roleUser in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleUser.Role.Name));
            }

            return claims;
        }

        //helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or only whitespaces", "password");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or only whitespaces.", "password");
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash. Should be 64 bytes");
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt. Should be 128 bytes");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
            return true;
        }
    }
}
