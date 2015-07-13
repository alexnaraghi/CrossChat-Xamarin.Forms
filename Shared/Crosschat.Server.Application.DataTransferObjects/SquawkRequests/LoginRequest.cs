using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using System.Xml.Serialization;
using System;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
	[XmlType(TypeName = "LI", Namespace = "")]
    public class LoginRequest : RequestBase
    {
		[XmlAttribute("PW")]
        public string Password { get; set; }
		[XmlAttribute("UN")]
		public string Name { get; set; }
    }

	[XmlType(TypeName="U", Namespace = "")]
	public class LoginResponse : ResponseBase
    {
		// ATTRIBUTES
		[XmlAttribute("SSID")]
		public long SSID  { get; set; }

		[XmlIgnore]
		public bool RWorldIsOn { get; set; }
		[XmlAttribute("RWorldIsOn")]
		public string RWorldIsOnString
		{
			get { return RWorldIsOn ? "true" : "false"; }
			set { RWorldIsOn = value == "true"; }
		}

		[XmlAttribute("UID")]
		public int UserId  { get; set; }

		[XmlAttribute("US")]
		public int US  { get; set; }

		[XmlAttribute("IS")]
		public string IS { get; set; }

		[XmlAttribute("UN")]
		public string Username { get; set; }

		[XmlAttribute("RD")]
		public string RegistrationDate { get; set; }

		[XmlAttribute("PW")]
		public string Password { get; set; }

		[XmlAttribute("KLs")]
		public string KnownLanguages  { get; set; }

		[XmlAttribute("PLs")]
		public string PracticingLanguages  { get; set; }

		[XmlAttribute("E")]
		public string Email { get; set; }

		[XmlAttribute("FN")]
		public string FirstName { get; set; }

		[XmlAttribute("LN")]
		public string LastName { get; set; }

		[XmlAttribute("L")]
		public string L { get; set; }

		[XmlAttribute("LID")]
		public int LocaleID  { get; set; }

		[XmlIgnore]
		public Gender Gender
		{
			get{return (Gender)GenderInt;}
			set{ GenderInt = (int)value; }
		}
		[XmlAttribute("G")]
		public int GenderInt  { get; set; }

		[XmlIgnore]
		public DateTime Birthday { get; set; }
		[XmlAttribute("BD")]
		public string BirthdayString
		{
			get { return Birthday.ToString("yyyy-MM-dd"); }
			set { Birthday = DateTime.ParseExact(value, "yyyy-MM-dd", null); }
		}

		[XmlAttribute("A")]
		public int Age  { get; set; }

		[XmlAttribute("D")]
		public string Description { get; set; }

		[XmlAttribute("XCM")]
		public int XCM  { get; set; }

		[XmlAttribute("XCC")]
		public int XCC  { get; set; }

		[XmlAttribute("NM")]
		public int NM  { get; set; }
    }

}
