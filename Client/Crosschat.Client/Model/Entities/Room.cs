using System;

namespace Crosschat.Client
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

		public override string ToString()
		{
			return string.Format("+{0} ({1})", RoomId, Name);
		}
	}
}

