using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DateflixMVC.Dtos;
using DateflixMVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DateflixMVC.Helpers;
using DateflixMVC.Models;
using DateflixMVC.Models.Profile;
using DateflixMVC.Services;

namespace DateflixMVC.Controllers.API
{
    //[Authorize]
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

            var duplicate = _context.Likes.Where(x => x.UserId == likeDto.UserId && x.LikedId == likeDto.LikedId);
            // The like already exists
            if (duplicate != null)
                return Ok();

            //var existingMatch = _context.Likes.AsQueryable().Where(x => x.UserId == likeDto.UserId && x.LikedId == likeDto.LikedId);
            var existingMatch = _context.Matches.Where(x => x.UserOneId == likeDto.UserId && x.UserTwoId == likeDto.LikedId
            || x.UserTwoId == likeDto.LikedId && x.UserOneId == likeDto.UserId);

            if (existingMatch.Any()) // If match already exists
            {
                return Ok(true);
            }

            var matchHappened = _context.Likes.SingleOrDefault(x => x.UserId == likeDto.LikedId && x.LikedId == likeDto.UserId);
            // If match occours, add new match
            if (matchHappened != null)
                _context.Matches.Add(new Match { UserOneId = likeDto.UserId, UserTwoId = likeDto.LikedId });
            
            var likesEntry = _context.Likes.Add(_mapper.Map<Likes>(likeDto));
            _context.SaveChanges();

            return Ok();
        }

        //GET: api/date/matches
        [HttpGet("matches")]
        public IActionResult GetMatches(int userId)
        {
            var userLiked = _context.Likes.AsQueryable().Where(x => x.UserId == userId).ToList();
            var likedUser = _context.Likes.AsQueryable().Where(x => x.LikedId == userId).ToList();

            var matchedUserIds = new List<int>();
            foreach (var like in userLiked)
            {
                if (likedUser.Any(x => x.UserId == like.LikedId))
                {
                    matchedUserIds.Add(like.LikedId);
                }
            }

            var likedUsers = new List<User>();

            foreach (var matchedUserId in matchedUserIds)
            {
                var user = _userService.GetByIdAsync(matchedUserId);
                if (user != null)
                {
                    likedUsers.Add(user.Result);
                }
            }

            return Ok(likedUsers);
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
