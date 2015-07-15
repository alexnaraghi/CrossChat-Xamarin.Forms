using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace SharedSquawk.Server.Application.DataTransferObjects.Requests
{
	[XmlType(TypeName = "UPDs", Namespace = "")]
    public class ChatUpdateRequest : RequestBase
    {
		[XmlAttribute("D")]
		public string Delay  { get; set; }
		[XmlAttribute("UID")]
		public int UserId  { get; set; }
		[XmlAttribute("GUID")]
		public string GUID  { get; set; }
		[XmlAttribute("LUPD")]
		public int LastServerUpdateId  { get; set; }
		[XmlAttribute("UPDC")]
		public int ClientUpdateId  { get; set; }
		[XmlAttribute("Rs")]
		public string ActiveRooms  { get; set; }
		[XmlElement("M")]
		public List<MessageDto> Messages{ get; set;}
		[XmlElement("CR")]
		public List<ChatResultDto> UserChatResponses  { get; set; }
		[XmlElement("E")]
		public List<RoomEntryRequestDto> EnteredRooms  { get; set; }
		[XmlElement("C")]
		public List<UserChatDto> UserChatRequests  { get; set; }
    }

	[XmlType(TypeName="UPDs", Namespace = "")]
	public class ChatUpdateResponse : ResponseBase
    {
		[XmlAttribute("LUPD")]
		public int ServerUpdateId{ get; set;}
		[XmlElement("UPD")]
		public List<UpdateDto> Updates;
    }

	[XmlType(TypeName="E", Namespace = "")]
	public class RoomEntryRequestDto
	{
		[XmlAttribute("R")]
		public string Room  { get; set; }
	}

	[XmlType(TypeName="UPD", Namespace = "")]
	public class UpdateDto
	{
		[XmlAttribute("ID")]
		public int ID  { get; set; }
		[XmlElement("U")]
		public UserDto User { get; set;}
		[XmlElement("DU")]
		public DisconnectedUserDto DisconnectedUser { get; set;}
		[XmlElement("FP")]
		public FPDto FP { get; set;}
		[XmlElement("M")]
		public MessageDto Message { get; set;}
		[XmlElement("CR")]
		public ChatResultDto UserChatResult { get; set;}
		[XmlElement("ER")]
		public EnteredRoomDto EnteredRoom { get; set;}
		[XmlElement("C")]
		public UserChatDto UserChatRequest { get; set;}
	}

	[XmlType(TypeName="CR", Namespace = "")]
	public class ChatResultDto
	{
		[XmlAttribute("UID")]
		public int UserId { get; set;}

		[XmlAttribute("V")]
		public string V { get; set;} //Value

		[XmlIgnore]
		public ChatReply Reply
		{
			get 
			{
				return (ChatReply)Enum.Parse (typeof(ChatReply), V);
			}
			set 
			{
				V = value.ToString ();
			}
		}
	}

	[XmlType(TypeName="DU", Namespace = "")]
	public class DisconnectedUserDto
	{
		[XmlAttribute("UID")]
		public int UserId { get; set;}
		[XmlAttribute("R")]
		public string RoomId { get; set;}
	}

	[XmlType(TypeName="M", Namespace = "")]
	public class MessageDto
	{
		[XmlAttribute("R")]
		public string Room  { get; set; }
		[XmlAttribute("UID")]
		public int UserId { get; set;}
		[XmlText]
		public string Text { get; set;}
	}

	[XmlType(TypeName="FP", Namespace = "")]
	public class FPDto
	{
		[XmlAttribute("R")]
		public string Room  { get; set; }
		[XmlAttribute("UID")]
		public int UserId { get; set;}
	}

	[XmlType(TypeName="NU", Namespace = "")]
	public class NewUserDto
	{
		[XmlAttribute("UID")]
		public int UserId { get; set;}
	}

	[XmlType(TypeName="C", Namespace = "")]
	public class UserChatDto
	{
		[XmlAttribute("UID")]
		public int UserId;
	}

	[XmlType(TypeName="ER", Namespace = "")]
	public class EnteredRoomDto
	{
		[XmlAttribute("R")]
		public string Room { get; set;}
		[XmlElement("NU")]
		public List<NewUserDto> NewUsers{ get; set;}
		[XmlElement("M")]
		public List<EnteredRoomMessageDto> RoomMessages{ get; set;}
	}

	[XmlType(TypeName="M", Namespace = "")]
	public class EnteredRoomMessageDto
	{
		[XmlElement("uN")]
		public UserNameDto User;
		[XmlElement("mO")]
		public MessageBodyDto Message;
	}

	[XmlType(TypeName="uN", Namespace = "")]
	public class UserNameDto
	{
		[XmlText]
		public string Name;
	}

	[XmlType(TypeName="mO", Namespace = "")]
	public class MessageBodyDto
	{
		[XmlText]
		public string Body;
	}
}
