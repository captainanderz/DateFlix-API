using System.Collections.Generic;
using System.Linq;
using DateflixMVC.Models.SignalR;

namespace DateflixMVC.Extensions
{
    public static class UserDetailExtension
    {
        public static List<UserDetail> AddUser(this List<UserDetail> userDetailsList, string email, string connectionId)
        {
            userDetailsList.Add(new UserDetail()
            {
                Email = email,
                ConnectionId = connectionId
            });

            return userDetailsList;
        }

        public static UserDetail GetUserByConnectionId(this List<UserDetail> userDetailsList, string connectionId)
        {
            return userDetailsList.FirstOrDefault(x => x.ConnectionId == connectionId);
        }

        public static void RemoveUser(this List<UserDetail> userDetailsList, string connectionId)
        {
            var user = userDetailsList.GetUserByConnectionId(connectionId);
            if (user != null)
            {
                userDetailsList.Remove(user);
            }
        }
    }
}
