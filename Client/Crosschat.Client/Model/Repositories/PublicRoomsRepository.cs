using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client.Model
{
    public class PublicRoomsRepository
    {
		private static readonly Room[] _rooms = 
			new[]
		{
			new Room("CREn", "English"),
			new Room("CREnJa", "English - Japanese"),
			new Room("CRJa", "Japanese"),
			new Room("CREnCh", "English - Chinese (Mandarin)"),
			new Room("CREnAr", "English - Arabic"),
			new Room("CREnKo", "English - Korean"),
			new Room("CREnFr", "English - French"),
			new Room("CREnRu", "English - Russian"),
			new Room("CREnPt", "English - Portugese"),
			new Room("CREnSp", "English - Spanish"),
			//TODO: Add all public rooms to this list
		};

        public static Room[] GetAll()
        {
			return _rooms;
        }
    }
}
