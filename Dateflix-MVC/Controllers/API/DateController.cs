using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DateflixMVC.Dtos;
using DateflixMVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using DateflixMVC.Helpers;
using DateflixMVC.Models;
using DateflixMVC.Models.Profile;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Authorization;

namespace DateflixMVC.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private readonly WebApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public DateController(WebApiDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("getMatchingUsers")]
        public IActionResult GetMatchingUsers(int userId)
        {
            var user = _userService.GetById(userId);

            if (user?.UserPreference == null)
            {
                return BadRequest("User or UserPreference doesnt exist");
            }

            var matchingUsers = _userService.GetAll()
                .Where(x => x.Gender == user.UserPreference.Gender
                && _userService.IsAgeInsideRange(x.Birthday.ToAge(), user.UserPreference)
                && x.UserPreference != null
                && x.UserPreference.Gender == user.Gender
                && _userService.IsAgeInsideRange(user.Birthday.ToAge(), x.UserPreference)).ToList();

            var usersDto = _mapper.Map<IEnumerable<User>, List<UserDto>>(matchingUsers);

            return Ok(usersDto);
        }

        //POST: api/date/like
        [HttpPost("like")]
        public IActionResult Like([FromBody]LikeDto likeDto)
        {
            if (likeDto == null)
            {
                return BadRequest();
            }

            likeDto.CreatedDate = DateTime.UtcNow;

            var existingMatch = _context.Matches.Where(x => x.UserOneId == likeDto.UserId && x.UserTwoId == likeDto.LikedId
            || x.UserTwoId == likeDto.LikedId && x.UserOneId == likeDto.UserId);

            if (existingMatch.Any()) // If match already exists
            {
                return Ok("Existing match");
            }

            var matchHappened = _context.Likes.SingleOrDefault(x => x.UserId == likeDto.LikedId && x.LikedId == likeDto.UserId);

            // If match occours, add new match and like
            if (matchHappened != null)
            { 
                _context.Matches.Add(new Match { UserOneId = likeDto.UserId, UserTwoId = likeDto.LikedId });
                _context.Likes.Add(_mapper.Map<Likes>(likeDto));
                _context.SaveChanges();
                return Ok("Match happened!");
            }

            var likesEntry = _context.Likes.Add(_mapper.Map<Likes>(likeDto));
            _context.SaveChanges();
            return Ok("Like recorded");
        }

        //GET: api/date/matches
        [HttpGet("matches")]
        public IActionResult GetMatches(int userId)
        {
            var matches = _context.Matches.Where(x => x.UserOneId == userId || x.UserTwoId == userId); // Fetch all of the matches
            var matchedUsers = new List<User>(); // This list will contain all the matched users
            foreach (var match in matches)
            {
                var user = _userService.GetById(match.UserOneId == userId ? match.UserTwoId : match.UserOneId); // If userId is UserOneId, search for opposite user in Db. Ie. search for the matched user or vice versa.
                matchedUsers.Add(user); 
            }
            var dtoMatchedUsers = _mapper.Map<IEnumerable<User>, List<UserDto>>(matchedUsers);
            return Ok(dtoMatchedUsers);
        }

        // GET: api/Date/5
        [HttpGet("{userId}")]
        public IActionResult GetLikes(int userId)
        {
            var likes = _context.Likes.Where(x => x.UserId == userId);
            return Ok(likes);
        }

        //GET: api/date/block
        [HttpGet("block")]
        public async Task<ActionResult> Block([FromBody]BlockDto blockDto)
        {
            if (blockDto == null)
            {
                return BadRequest();
            }

            var existingBlock = _context.Blocks.AsQueryable().FirstOrDefault(x => x.UserId == blockDto.UserId && x.BlockedUserId == blockDto.BlockedUserId);
            if (existingBlock != null)
            {
                return Ok(existingBlock);
            }

            await _context.Blocks.AddAsync(_mapper.Map<Blocks>(blockDto));

            return Ok();
        }
    }
}
