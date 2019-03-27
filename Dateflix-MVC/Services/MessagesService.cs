using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
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

        public async Task SaveMessage(string senderEmail, string receiverEmail, string message)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(receiverEmail))
            {
                return;
            }

            var senderUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == senderEmail);
            var receiverUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == receiverEmail);

            if (senderUser == null || receiverUser == null)
            {
                return;
            }

            await SaveMessage(senderUser, receiverUser.Id, message);
        }

        public async Task SaveMessage(User sender, int receiverId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var directMessage = new DirectMessages()
            {
                SenderId = sender.Id,
                ReceiverId = receiverId,
                Message = message,
                CreatedDate = DateTime.UtcNow
            };

            _context.DirrectMessages.Add(directMessage);
            await _context.SaveChangesAsync();
        }

        public List<MessageDto> GetMessages(string senderEmail, string receiverEmail)
        {
            var senderUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == senderEmail);
            var receiverUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == receiverEmail);

            if (senderUser == null || receiverUser == null)
            {
                return null;
            }

            var senderMessages = _context.DirrectMessages.AsQueryable().Where(x => x.SenderId == senderUser.Id && x.ReceiverId == receiverUser.Id).ToList();
            var receiverMessages = _context.DirrectMessages.AsQueryable().Where(x => x.SenderId == receiverUser.Id && x.ReceiverId == senderUser.Id).ToList();

            var allMessages = senderMessages;
            allMessages.AddRange(receiverMessages);
            var ordered = allMessages.OrderBy(x => x.CreatedDate).ToList();

            var messageDtos = new List<MessageDto>();
            foreach (var directMessage in ordered)
            {
                messageDtos.Add(new MessageDto()
                {
                    Date = directMessage.CreatedDate,
                    Message = directMessage.Message,
                    ReceiverFirstname = receiverUser.Id == directMessage.ReceiverId ? receiverUser.FirstName : senderUser.FirstName,
                    SenderFirstname = senderUser.Id == directMessage.SenderId ? senderUser.FirstName : receiverUser.FirstName
                });
            }

            return messageDtos;
        }
    }
}
