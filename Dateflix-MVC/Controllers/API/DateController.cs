using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DateflixMVC.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/Date
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Likes>>> GetLikes()
        {
            return await _context.Likes.ToListAsync();
        }

        [HttpPost("like")]
        public ActionResult Like([FromBody]LikeDto likeDto)
        {
            if (likeDto == null)
            {
                return BadRequest();
            }

            var like = new LikeDto()
            {
                UserId = likeDto.UserId,
                LikedId = likeDto.LikedId,
                CreatedDate = DateTime.UtcNow
            };

            var likesEntry = _context.Likes.Add(_mapper.Map<Likes>(like));
            _context.SaveChanges();
            var match = _context.Likes.AsQueryable().FirstOrDefault(x => x.UserId == like.LikedId && x.LikedId == like.UserId);

            if (match != null)
            {
                return Content("Match");
            }

            return likesEntry == null ? StatusCode(500) : Ok();
        }

        [HttpGet("matches")]
        public ActionResult GetMatches(int userId)
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
            //Parallel.ForEach(matchedUserIds, async (matchedUserId) =>
            //{
            //    var user = await _userService.GetByIdAsync(matchedUserId);
            //    if (user != null)
            //    {
            //        likedUsers.Add(user);
            //    }
            //});

            return Ok(likedUsers);
        }

        // GET: api/Date/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Likes>> GetLikes(int id)
        {
            var likes = await _context.Likes.FindAsync(id);

            if (likes == null)
            {
                return NotFound();
            }

            return likes;
        }

        // PUT: api/Date/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLikes(int id, Likes likes)
        {
            if (id != likes.Id)
            {
                return BadRequest();
            }

            _context.Entry(likes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Date
        [HttpPost]
        public async Task<ActionResult<Likes>> PostLikes(Likes likes)
        {
            _context.Likes.Add(likes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLikes", new { id = likes.Id }, likes);
        }

        // DELETE: api/Date/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Likes>> DeleteLikes(int id)
        {
            var likes = await _context.Likes.FindAsync(id);
            if (likes == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(likes);
            await _context.SaveChangesAsync();

            return likes;
        }

        private bool LikesExists(int id)
        {
            return _context.Likes.Any(e => e.Id == id);
        }
    }
}
