using System.Collections.Generic;
using System.Threading.Tasks;
using DateflixMVC.Dtos;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IMessageService
    {
        List<MessageDto> GetMessages(string senderUsername, string receiverUsername, string senderId = null, string receiverId = null);
        Task SaveMessage(string senderUsername, string receiverUsername, string message);
        Task SaveMessage(User sender, int receiverId, string message);
    }
}
