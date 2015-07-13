using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
	[XmlType(TypeName = "U", Namespace = "")]
    public class ConnectedMembersRequest : RequestBase
    {
		[XmlAttribute("SSID")]
		public long SSID  { get; set; }
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

	[XmlType(TypeName="UI", Namespace = "")]
	public class ConnectedMembersResponse : ResponseBase
    {
		[XmlAttribute("UID")]
		public int UserId{ get; set;}
		[XmlAttribute("GUID")]
		public string GUID{ get; set;}
		[XmlElement("U")]
		public List<UserDto> Users;
    }
}
