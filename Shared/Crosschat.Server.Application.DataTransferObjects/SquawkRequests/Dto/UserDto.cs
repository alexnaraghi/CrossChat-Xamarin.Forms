using System;
using System.Xml.Serialization;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
	[XmlType(TypeName="U", Namespace = "")]
	public class UserDto
	{
		[XmlAttribute("PLs")]
		public string PracticingLanguages  { get; set; }
		[XmlAttribute("KLs")]
		public string KnownLanguages  { get; set; }
		[XmlAttribute("LID")]
		public int LocaleID  { get; set; }
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
		[XmlAttribute("LN")]
		public string LastName  { get; set; }
		[XmlAttribute("FN")]
		public string FirstName  { get; set; }
		[XmlAttribute("UID")]
		public int UserID  { get; set; }
	}
}

