using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IInquiryService
    {
        Inquiries GetById(int id);
        Inquiries SubmitInquiry(Inquiries inquiry);
        Inquiries DeleteInquiry(Inquiries inquiry);
    }
}
