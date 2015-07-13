using SharedSquawk.Server.Application.DataTransferObjects.Messages;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
    public class GetOnlineUsersRequest : RequestBase { }
    public class GetOnlineUsersResponse : ResponseBase
    {
        public UserDto[] Users { get; set; }
    }
}