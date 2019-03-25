using System.Collections.Generic;
using System.Linq;
using DateflixMVC.Models.SignalR;

namespace DateflixMVC.Extensions
{
    public static class UserDetailExtension
    {
        public static List<UserDetail> AddConnectionIdToUser(this List<UserDetail> userDetailsList, string email, string connectionId)
        {
            var userToUpdate = userDetailsList.FirstOrDefault(x => x.Email == email);

            if (userToUpdate != null)
            {
                userToUpdate.ConnectionId = connectionId;
            }

            return userDetailsList;
        }

        public static bool HasEmail(this List<UserDetail> userDetailsList, string email)
        {
            return userDetailsList.Any(x => x.Email.ToLower() == email.ToLower());
        }

        public static UserDetail GetUserByConnectionId(this List<UserDetail> userDetailsList, string connectionId)
        {
            return userDetailsList.FirstOrDefault(x => x.ConnectionId == connectionId);
        }

        public static void RemoveUser(this List<UserDetail> userDetailsList, string connectionId)
        {
            var key = userDetailsList.GetUserByConnectionId(connectionId);
            if (key != null)
            {
                userDetailsList.Remove(key);
            }
        }
    }
}
