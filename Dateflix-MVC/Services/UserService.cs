using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DateflixMVC.Services
{
    public class UserService : IUserService
    {
        private WebApiDbContext _context;
        private readonly AppSettings _appSettings;

        public UserService(WebApiDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public UserDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.Include(x => x.Roles).ThenInclude(x => x.Role).SingleOrDefault(u => u.Email == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var userAndRolesClaims = new List<Claim>
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetUserClaims(user)),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.UtcNow.AddMinutes(1)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            };
        }

        public User Create(User user, string password)
        {
            // Validation..
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("Email " + user.Email + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedDate = DateTime.UtcNow;

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

        public List<User> GetAll()
        {
            return _context.Users.AsQueryable().Include(x => x.UserPreference).Include(x => x.Likes).ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.AsQueryable().Include(x => x.UserPreference).Include(x => x.Roles).ThenInclude(x => x.Role).Include(x => x.Roles).ThenInclude(x => x.User).SingleOrDefault(x => x.Id == id);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            //return await _context.Users.FindAsync(id);
            return _context.Users.AsQueryable().Include(x => x.UserPreference).Include(x => x.Roles).ThenInclude(x => x.Role).Include(x => x.Roles).ThenInclude(x => x.User).SingleOrDefault(x => x.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.AsQueryable().Include(x => x.UserPreference).Include(x => x.Roles).ThenInclude(x => x.Role).Include(x => x.Roles).ThenInclude(x => x.User).SingleOrDefault(x => x.Email == username);
        }

        public void Update(User user, string password = null)
        {
            var userInDb = _context.Users.Find(user.Id);

            if (userInDb == null)
            {
                throw new AppException("User not found");
            }

            if (user.Email != userInDb.Email)
            {
                // username changed, checking for availability
                if (_context.Users.Any(x => x.Email == user.Email))
                {
                    throw new AppException("Email " + user.Email + " is already taken");
                }
            }

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.Email = user.Email;
            userInDb.Birthday = user.Birthday;
            userInDb.City = user.City;
            userInDb.ProfilePictures = user.ProfilePictures;
            userInDb.Gender = user.Gender;
            userInDb.UpdatedDate = DateTime.UtcNow;
            userInDb.Description = user.Description;
            userInDb.UserPreference = user.UserPreference;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                userInDb.PasswordHash = passwordHash;
                userInDb.PasswordSalt = passwordSalt;
            }
            _context.Users.Update(userInDb);
            _context.SaveChanges();
        }

        public bool UpdateUserPreference(int id, UserPreference userPreference)
        {
            var userInDb = GetById(id);

            if (userInDb == null)
            {
                return false;
            }

            userInDb.UserPreference = userPreference;

            _context.Users.Update(userInDb);
            _context.SaveChanges();

            return true;
        }

        // generates and returns an IEnumerable of claims including the all of the user's roles and their Id.
        public IEnumerable<Claim> GetUserClaims(User user)
        {
            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };
            // Add claim for user's id

            // Add claim for each of the users roles
            foreach (var roleUser in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleUser.Role.Name));
            }

            return claims;
        }

        #region Private helpers

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

        #endregion
    }
}
