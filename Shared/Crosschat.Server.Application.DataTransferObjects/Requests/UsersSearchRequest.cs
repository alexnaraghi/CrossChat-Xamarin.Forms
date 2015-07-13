using SharedSquawk.Server.Application.DataTransferObjects.Messages;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
    public class UsersSearchRequest : RequestBase
    {
        public string QueryString { get; set; }
    }
    public class UsersSearchResponse : ResponseBase
    {
        public UserDto[] Result { get; set; }
    }
}
