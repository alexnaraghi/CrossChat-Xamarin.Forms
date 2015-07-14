using System;

namespace SharedSquawk.Client.Model.Entities
{
	public class Room
	{
		public Room(string roomId, string name )
		{
			RoomId = roomId;
			Name = name;
		}

		public string RoomId { get; private set; }

		public string Name { get; private set; }

		//Optional field to specify a user
		public int? UserId { get; set; }

		public bool IsUserChat
		{
			get
			{
				return UserId != null;
			}
		}

		public override string ToString()
		{
			return string.Format("+{0} ({1})", RoomId, Name);
		}
	}
}

