using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using System.Xml.Serialization;
using System;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
	[XmlType(TypeName = "U", Namespace = "")]
	public class ProfileRequest : RequestBase
	{
		[XmlAttribute("UID")]
		public int UserId { get; set; }
	}

	[XmlType(TypeName="P", Namespace = "")]
	public class ProfileResponse : ResponseBase
	{
		// ATTRIBUTES

		//this sends back accepted... why?
		[XmlAttribute("US")]
		public string US  { get; set; }

		[XmlAttribute("UID")]
		public int UserId  { get; set; }


		[XmlAttribute("FN")]
		public string FirstName { get; set; }

		[XmlAttribute("LN")]
		public string LastName { get; set; }

		[XmlAttribute("LID")]
		public int LocaleID  { get; set; }


		[XmlAttribute("L")]
		public string L { get; set; }

		[XmlAttribute("A")]
		public int Age  { get; set; }

		[XmlIgnore]
		public Gender Gender
		{
			get{return (Gender)GenderInt;}
			set{ GenderInt = (int)value; }
		}
		[XmlAttribute("G")]
		public int GenderInt  { get; set; }

		[XmlAttribute("KLs")]
		public string KnownLanguages  { get; set; }

		[XmlAttribute("PLs")]
		public string PracticingLanguages  { get; set; }

		[XmlAttribute("D")]
		public string Description { get; set; }

		[XmlAttribute("XCM")]
		public int XCM  { get; set; }

		[XmlAttribute("XCC")]
		public int XCC  { get; set; }

		[XmlAttribute("RD")]
		public string RegistrationDate { get; set; }

		//All of these  things are in our user but not other user profiles
//		[XmlAttribute("IS")]
//		public string IS { get; set; }
//
//		[XmlAttribute("UN")]
//		public string Username { get; set; }
//
//		[XmlAttribute("PW")]
//		public string Password { get; set; }
//
//		[XmlAttribute("E")]
//		public string Email { get; set; }
//
//		[XmlIgnore]
//		public DateTime Birthday { get; set; }
//		[XmlAttribute("BD")]
//		public string BirthdayString
//		{
//			get { return Birthday.ToString("yyyy-MM-dd"); }
//			set { Birthday = DateTime.ParseExact(value, "yyyy-MM-dd", null); }
//		}
//
//		[XmlAttribute("NM")]
//		public int NM  { get; set; }
	}

}
