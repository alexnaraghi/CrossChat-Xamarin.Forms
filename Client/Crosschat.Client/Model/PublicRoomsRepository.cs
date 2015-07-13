using Crosschat.Client.Model.Entities;

namespace Crosschat.Client.Model
{
    public class PublicRoomsRepository
    {
		private static readonly Room[] _rooms = 
			new[]
		{
			new Room("CREn", "English"),
			new Room("CREnJa", "English - Japanese"),
			new Room("CRJa", "Japanese"),
			new Room("CREnCh", "English - Chinese (Mandarin)")
			//TODO: Add all public rooms to this list
		};

        public static Room[] GetAll()
        {
			return _rooms;
        }
    }
}
