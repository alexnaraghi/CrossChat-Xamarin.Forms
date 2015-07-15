using SharedSquawk.Client.Model.Entities.Messages;
using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client
{
    public class RoomFactory
    {
		public static Room Get(int currentUserId, int otherUserId, string otherUserName)
        {
			var room = new Room (GetRoomId(currentUserId, otherUserId), otherUserName);
			room.UserId = otherUserId;

			return room;
        }

		public static string GetRoomId(int creatorUserId, int otherUserId)
		{
			return creatorUserId + "-" + otherUserId;
		}
    }
}
