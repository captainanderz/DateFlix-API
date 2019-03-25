using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Dtos
{
    public class InquiryDto
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
