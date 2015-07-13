using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using System.Xml.Serialization;
using System;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
	[XmlType(TypeName = "U", Namespace = "")]
    public class MemberStatusRequest : RequestBase
    {
		[XmlAttribute("SSID")]
        public long SessionId { get; set; }
		[XmlAttribute("UID")]
		public int UserId { get; set; }
    }

	[XmlType(TypeName="U", Namespace = "")]
	public class MemberStatusResponse : ResponseBase
    {
		[XmlAttribute("US")]
		public int US { get; set; }
		[XmlIgnore]
		public bool RWorldIsOn { get; set; }
		[XmlAttribute("RWorldIsOn")]
		public string RWorldIsOnString
		{
			get { return RWorldIsOn ? "true" : "false"; }
			set { RWorldIsOn = value == "true"; }
		}
    }

}
