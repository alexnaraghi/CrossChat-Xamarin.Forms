using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using System.Xml.Serialization;
using System;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class LoginRequest : RequestBase
    {
		[XmlAttribute]
        public string Name { get; set; }
		[XmlAttribute]
        public string Password { get; set; }
    }
	public class LoginResponse : ResponseBase
    {
		[XmlAttribute("U")]
		public U U;
    }

	public class U
	{
		// ATTRIBUTES
		[XmlAttribute("SSID")]
		public int SSID  { get; set; }

		[XmlIgnore]
		public bool RWorldIsOn { get; set; }
		[XmlAttribute("RWorldIsOn")]
		public string RWorldIsOnString
		{
			get { return RWorldIsOn ? "true" : "false"; }
			set { RWorldIsOn = value == "true"; }
		}

		[XmlAttribute("UID")]
		public int UID  { get; set; }

		[XmlAttribute("US")]
		public int US  { get; set; }

		[XmlAttribute("IS")]
		public string IS { get; set; }

		[XmlAttribute("UN")]
		public string UN { get; set; }

		[XmlAttribute("RD")]
		public string RD { get; set; }

		[XmlAttribute("PW")]
		public string PW { get; set; }

		[XmlAttribute("KLs")]
		public int KLs  { get; set; }

		[XmlAttribute("PLs")]
		public int PLs  { get; set; }

		[XmlAttribute("E")]
		public string E { get; set; }

		[XmlAttribute("FN")]
		public string FN { get; set; }

		[XmlAttribute("LN")]
		public string LN { get; set; }

		[XmlAttribute("L")]
		public string L { get; set; }

		[XmlAttribute("LID")]
		public int LID  { get; set; }

		[XmlAttribute("G")]
		public int G  { get; set; }

		[XmlIgnore]
		public DateTime BD { get; set; }
		[XmlAttribute("BD")]
		public string BDString
		{
			get { return BD.ToString("yyyy-MM-dd"); }
			set { BD = DateTime.ParseExact(value, "yyyy-MM-dd", null); }
		}

		[XmlAttribute("A")]
		public int A  { get; set; }

		[XmlAttribute("D")]
		public string D { get; set; }

		[XmlAttribute("XCM")]
		public int XCM  { get; set; }

		[XmlAttribute("XCC")]
		public int XCC  { get; set; }

		[XmlAttribute("NM")]
		public int NM  { get; set; }
	}

}
