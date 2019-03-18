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

        public async Task SaveMessage(string senderUsername, string receiverUsername, string message)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(senderUsername) || string.IsNullOrWhiteSpace(receiverUsername))
            {
                return;
            }

            var senderUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == senderUsername);
            var receiverUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == receiverUsername);

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

        public List<MessageDto> GetMessages(string senderUsername, string receiverUsername, string senderId = null, string receiverId = null)
        {
            if (senderId == null || receiverId == null)
            {
                var senderUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == senderUsername);
                var receiverUser = _context.Users.AsQueryable().FirstOrDefault(x => x.Email == receiverUsername);

                if (senderUser == null || receiverUser == null)
                {
                    return null;
                }

                senderId = senderUser.Id.ToString();
                receiverId = receiverUser.Id.ToString();
            }

            var senderMessages = _context.DirrectMessages.AsQueryable().Where(x => x.SenderId.ToString() == senderId && x.ReceiverId.ToString() == receiverId).ToList();
            var receiverMessages = _context.DirrectMessages.AsQueryable().Where(x => x.SenderId.ToString() == receiverId && x.ReceiverId.ToString() == senderId).ToList();

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
                    ReceiverFirstname = receiverId == directMessage.ReceiverId.ToString() ? receiverUsername : senderUsername,
                    SenderFirstname = senderId == directMessage.SenderId.ToString() ? senderUsername : receiverUsername
                });
            }

            return messageDtos;
        }
    }
}
