using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DateflixMVC.Dtos;
using DateflixMVC.Helpers;
using DateflixMVC.Models;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DateflixMVC.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly WebApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AdminController(WebApiDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Bans>>> GetAllBans()
        {
            return await _context.Bans.ToListAsync();
        }

        //GET: api/date/ban
        [HttpPost("ban")]
        public async Task<ActionResult> Ban([FromBody]BanDto banDto)
        {
            if (banDto == null)
            {
                return BadRequest();
            }

            banDto.CreatedDate = DateTime.UtcNow;

            var existingBlock = _context.Bans.AsQueryable().FirstOrDefault(x => x.Email == banDto.Email);
            if (existingBlock != null)
            {
                return Ok(existingBlock);
            }

            await _context.Bans.AddAsync(_mapper.Map<Bans>(banDto));
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
