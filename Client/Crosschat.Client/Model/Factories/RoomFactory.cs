using SharedSquawk.Client.Model.Entities.Messages;
using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client
{
    public class RoomFactory
    {
		public static Room Get(int otherUserId, string otherUserName, int currentUserId)
        {
			var room = new Room (currentUserId + "-" + otherUserId, otherUserName);
			room.UserId = otherUserId;

			return room;
        }
    }
}
