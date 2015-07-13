using SharedSquawk.Server.Application.DataTransferObjects.Messages;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
    public class UserFriendsRequest : RequestBase
    {
    }
    public class UserFriendsResponse : ResponseBase
    {
        public UserDto[] Friends { get; set; }
    }
}