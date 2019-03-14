using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateflixMVC.Dtos
{
    public class MessageDto
    {
        public string SenderFirstname { get; set; }
        public string ReceiverFirstname { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
