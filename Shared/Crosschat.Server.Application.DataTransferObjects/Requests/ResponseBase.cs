using System.Xml.Serialization;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            RequestResult = true;
        }

        //In order to associate Request with response
		[XmlIgnore]
        public long Token { get; set; }

        //false means timeout or connection close
		[XmlIgnore]
        public bool RequestResult { get; set; }

		[XmlIgnore]
        public CommonErrors Error { get; set; }
    }

    public enum CommonErrors
    {
        //negative values are not errors ;)
        Success = 0,
        Maintenance,
        Banned,
    }
}