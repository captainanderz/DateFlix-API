using System;
using System.Collections.Generic;
using System.Linq;
using DateflixMVC.Helpers;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public class MessagesService : IMessageService
    {
        private WebApiDbContext _context;

        public MessagesService(WebApiDbContext context)
        {
            _context = context;
        }

        public bool SaveMessage(string senderId, string receiverId, string message)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(receiverId))
            {
                return false;
            }

            var directMessage = new DirectMessages()
            {
                SenderId = Int32.Parse(senderId),
                ReceiverId = Int32.Parse(receiverId),
                Message = message,
                CreatedDate = DateTime.UtcNow
            };

            _context.DirrectMessages.Add(directMessage);
            _context.SaveChanges();

            return true;
        }

        public List<DirectMessages> GetMessages(string senderId, string receiverId)
        {
            var senderMessages = _context.DirrectMessages.Where(x => x.SenderId.ToString() == senderId && x.ReceiverId.ToString() == receiverId).ToList();
            var receiverMessages = _context.DirrectMessages.Where(x => x.SenderId.ToString() == receiverId && x.ReceiverId.ToString() == senderId).ToList();

            var allMessages = senderMessages;
            allMessages.AddRange(receiverMessages);
            return allMessages.OrderByDescending(x => x.CreatedDate).ToList();
        }
    }
}
