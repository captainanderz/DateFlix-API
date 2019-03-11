using System.Collections.Generic;
using DateflixMVC.Models.Profile;

namespace DateflixMVC.Services
{
    public interface IMessageService
    {
        bool SaveMessage(string senderId, string receiverId, string message);
        List<DirectMessages> GetMessages(string senderId, string receiverId);
    }
}
