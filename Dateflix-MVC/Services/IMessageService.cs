using System.Collections.Generic;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IMessageService
    {
        List<MessageDto> GetMessages(string senderEmail, string receiverEmail, string senderId = null, string receiverId = null);
        Task SaveMessage(string senderEmail, string receiverEmail, string message);
        Task SaveMessage(User sender, int receiverId, string message);
    }
}
