using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DateflixMVC.Dtos;
using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DateflixMVC.Controllers.API
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InquiriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly WebApiDbContext _context;
        private readonly IInquiryService _inquiryService;

        public InquiriesController(IMapper mapper, WebApiDbContext context, IInquiryService inquiryService)
        {
            _mapper = mapper;
            _context = context;
            _inquiryService = inquiryService;
        }

        public IActionResult Test()
        {
            return Ok(_context.Inquiries.ToList());
        }

        [HttpPost("submit")]
        public IActionResult SubmitInquiry([FromBody]InquiryDto inquiryDto)
        {
            var inquiry = _mapper.Map<Inquiries>(inquiryDto); // Convert dto to model
            inquiry = _inquiryService.SubmitInquiry(inquiry); // Call service to save the inquiry. Override inquiry with returned object

            if (inquiry != null) // If returned object is not null, the submit succeeded
                return Ok();
            else // something went wrong
                return Content("Failed to save message");
        }
    }
}
