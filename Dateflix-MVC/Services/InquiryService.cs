using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;
using System;

namespace DateflixMVC.Services
{
    public class InquiryService : IInquiryService
    {
        private readonly WebApiDbContext _context;

        public InquiryService(WebApiDbContext context)
        {
            _context = context;
        }

        public Inquiries DeleteInquiry(Inquiries inquiry)
        {
            throw new System.NotImplementedException();
        }

        public Inquiries GetById(int id)
        {
            throw new System.NotImplementedException();
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
    }
}
