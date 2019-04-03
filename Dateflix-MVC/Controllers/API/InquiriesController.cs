using AutoMapper;
using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DateflixMVC.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InquiriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IInquiryService _inquiryService;

        public InquiriesController(IMapper mapper, IInquiryService inquiryService)
        {
            _mapper = mapper;
            _inquiryService = inquiryService;
        }
        
        [HttpGet("{id}")]
        public IActionResult GetInquiryById(int id)
        {
            var inquiry = _inquiryService.GetById(id); // Fetches inquiry by it's id
            if (inquiry == null) 
                return NoContent(); // Return 204 if inquiry is null

            return Ok(inquiry);
        }

        [HttpGet("userinquiries/{userId}")]
        public IActionResult GetUserInquiries(int userId)
        {
            var usersInquiries = _inquiryService.GetInquiriesByUserId(userId); // Fetches users inquiries
            if (usersInquiries == null)
                return NoContent(); // return 204, if list is null

            return Ok(usersInquiries);
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

        [HttpDelete("{id}")]
        public IActionResult DeleteInquiry(int id)
        {
            var inquiry = _inquiryService.GetById(id); // Fetch inquiry by id
            if (inquiry == null)
                return BadRequest(); // if inquiry doesent exists, return badrequest

            _inquiryService.DeleteInquiry(inquiry); // delete inquiry
            return Ok();
        }
    }
}
