using System.Xml.Serialization;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            Error = CommonErrors.Success;
        }

        //In order to associate Request with response
		[XmlIgnore]
        public long Token { get; set; }

        //false means timeout or connection close
        public bool RequestResult { get { return Error == CommonErrors.Success; }}

		[XmlIgnore]
        public CommonErrors Error { get; set; }
    }

    public enum CommonErrors
    {
        //negative values are not errors ;)
        Success = 0,
        Timeout,
        AuthenticationFailure,
        Maintenance,
        Banned,
		Unknown
    }
}