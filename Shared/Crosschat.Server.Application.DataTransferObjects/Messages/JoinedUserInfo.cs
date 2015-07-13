using SharedSquawk.Server.Application.DataTransferObjects.Requests;

namespace SharedSquawk.Server.Application.DataTransferObjects.Messages
{
    public class JoinedUserInfo : BaseDto
    {
        public UserDto User { get; set; }
    }

    public class LeftUserInfo : BaseDto
    {
        public int UserId { get; set; }
    }
}
