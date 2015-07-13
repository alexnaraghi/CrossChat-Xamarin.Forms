using SharedSquawk.Server.Application.DataTransferObjects.Messages;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
    public class UserBlacklistRequest : RequestBase
    {
    }
    public class UserBlacklistResponse : ResponseBase
    {
        public UserDto[] Blacklist { get; set; }
    }
}