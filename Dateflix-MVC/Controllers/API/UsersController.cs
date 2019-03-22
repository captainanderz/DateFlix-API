using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DateflixMVC.Dtos;
using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DateflixMVC.Controllers.API
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Email, userDto.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Email or password is incorrect" });
            }

            return Ok(user);
        }

        [HttpGet]
        public IActionResult Logout(int id)
        {
            

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            var mappedUser = _mapper.Map<User>(userDto);
            
            var user = _userService.Create(mappedUser, userDto.Password);
            return Ok(user);
        }


        [HttpGet("getall")]
        //[Authorize(Roles = "Captain")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        //[Authorize]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(user);

            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpPost("update")]
        public IActionResult Update(int userId, [FromBody]UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = userId;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _userService.Update(user, userDto.Password);
            return Ok();
        }

        [HttpPost("updateprofilepictures")]
        public async Task<IActionResult> UpdateProfilePictures(int userId, [FromForm]UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = userId;

            if (Request.Form.Files.Any())
            {
                foreach (var file in Request.Form.Files)
                {
                    var serverSavePath = Path.Combine(Directory.GetCurrentDirectory(), "profilePictures", file.FileName);

                    using (var stream = new FileStream(serverSavePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (user.ProfilePictures.Contains(serverSavePath))
                    {
                        continue;
                    }

                    var pictures = user.ProfilePictures.ToList();
                    pictures.Add(serverSavePath);
                    user.ProfilePictures = pictures.ToArray();
                }
            }

            _userService.Update(user);
            return Ok();
        }

        [HttpGet("downloadprofilepicture")]
        public async Task<IActionResult> DownloadProfilePicture(int userId, [FromBody]UserDto userDto)
        {
            var user = _userService.GetById(userId);
            user.Id = userId;

            var picturePath = userDto.ProfilePictures[0];
            var memory = new MemoryStream();
            using (var stream = new FileStream(picturePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, GetContentType(picturePath), Path.GetFileName(picturePath));
        }

        [HttpPost("UpdateUserPreference")]
        public IActionResult UpdateUserPreference(int userId, [FromBody] UserPreferenceDto userPreferenceDto)
        {
            var userPreference = _mapper.Map<UserPreference>(userPreferenceDto);
            var result = _userService.UpdateUserPreference(userId, userPreference);

            return result ? Ok() : StatusCode(500);
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            _userService.Delete(userId);
            return Ok();
        }

        #region PrivateMethods

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion
    }
}