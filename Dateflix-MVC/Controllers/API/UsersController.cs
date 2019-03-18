using System.Collections.Generic;
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
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);

            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _userService.Update(user, userDto.Password);
            return Ok();
        }

        [HttpPost("{id}")]
        public IActionResult UpdateUserPreference(int id, [FromBody] UserPreferenceDto userPreferenceDto)
        {
            var userPreference = _mapper.Map<UserPreference>(userPreferenceDto);
            var result = _userService.UpdateUserPreference(id, userPreference);

            return result ? Ok() : StatusCode(500);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

    }
}