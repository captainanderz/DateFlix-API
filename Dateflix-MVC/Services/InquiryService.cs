using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DateflixMVC.Services
{
    public class InquiryService : IInquiryService
    {
        private readonly WebApiDbContext _context;

        public InquiryService(WebApiDbContext context)
        {
            _context = context;
        }

        public Inquiries GetById(int id)
        {
            var inquiry = _context.Inquiries.Find(id); // Find inquiry by id

            if (inquiry == null) // if inquiry doesen't exist, return null.
                return null;

            return inquiry;
        }

        public IEnumerable<Inquiries> GetInquiriesByUserId(int userId)
        {
            var inquiries = _context.Inquiries.Where(x => x.UserId == userId); // Returns all inquiries with the given userid

            if (inquiries.Count() == 0) // If list is empty, return null
                return null;

            return inquiries;
        }

        public Inquiries SubmitInquiry(Inquiries inquiry)
        {
            // Error handling
            if (inquiry == null || inquiry.UserId == 0)
                return null;
            else if (string.IsNullOrWhiteSpace(inquiry.Message))
                return null;

            //Set createddate timestamp
            inquiry.CreatedDate = DateTime.UtcNow;

            //Add inquiry and save changes to database
            _context.Inquiries.Add(inquiry);
            _context.SaveChanges();

            return inquiry;
        }

        public Inquiries DeleteInquiry(Inquiries inquiry)
        {
            throw new System.NotImplementedException();
        }
    }
}
