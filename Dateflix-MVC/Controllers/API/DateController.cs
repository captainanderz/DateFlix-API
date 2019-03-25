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

            var matchingUsers = _userService.GetAll().Where(x => x.Gender == user.UserPreference.Gender && UserPreferenceHelper.IsInsideRange(x.Birthday.ToAge(), user.UserPreference)
                                                                 && x.UserPreference != null && x.UserPreference.Gender == user.Gender && UserPreferenceHelper.IsInsideRange(user.Birthday.ToAge(), x.UserPreference)).ToList();
            var usersDto = _mapper.Map<IEnumerable<User>, List<UserDto>>(matchingUsers);

            if (!usersDto.Any())
            {
                return Ok("No matching users");
            }

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

            var existingMatch = _context.Likes.AsQueryable().Where(x => x.UserId == likeDto.UserId && x.LikedId == likeDto.LikedId);

            if (existingMatch.Any())
            {
                return Ok();
            }

            var likesEntry = _context.Likes.Add(_mapper.Map<Likes>(likeDto));
            _context.SaveChanges();
            var match = _context.Likes.AsQueryable().FirstOrDefault(x => x.UserId == likeDto.LikedId && x.LikedId == likeDto.UserId);

            if (match != null)
            {
                return Ok(true);
            }

            return likesEntry == null ? StatusCode(500) : Ok();
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

            if (likes == null)
            {
                return NotFound();
            }

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
