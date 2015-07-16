using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client.Model
{
    public class PublicRoomsRepository
    {
		private static readonly Room[] _rooms = 
			new[]
		{
			new Room("CREnSp", "English - Spanish"),
			new Room("CREnJa", "English - Japanese"),
			new Room("CREnPt", "English - Portugese"),
			new Room("CREnCh", "English - Chinese (Mandarin)"),
			new Room("CREnRu", "English - Russian"),
			new Room("CREnFr", "English - French"),
			new Room("CREnKo", "English - Korean"),
			new Room("CREnAr", "English - Arabic"),
			new Room("CREnIt", "English - Italian"),
			new Room("CREnGe", "English - German"),
			new Room("CREnTu", "English - Turkish"),
			new Room("CREn", "English"),
			new Room("CRSp", "Spanish"),
			new Room("CRFr", "French"),
			new Room("CRJa", "Japanese"),
			new Room("CRGe", "German"),
			new Room("CRIt", "Italian"),
			new Room("CRCh", "Chinese (Mandarin)"),
			new Room("CRRu", "Russian"),
			new Room("CRPt", "Portugese"),
			new Room("CRAr", "Arabic"),
			new Room("CRKo", "Korean"),
			new Room("CRHi", "Hindi"),
			new Room("CRCa", "Chinese (Cantonese)"),
			new Room("CRDu", "Dutch"),
			new Room("CRTu", "Turkish"),
			new Room("CRSw", "Swedish"),
			new Room("CRPe", "Persian (Farsi)"),
			new Room("CRTh", "Thai"),
			new Room("CRPo", "Polish"),
			new Room("CRHu", "Hungarian"),
			new Room("CROther", "Multi-Language"),
			new Room("CRSca", "Scandanavian Languages"),
		};

        public static Room[] GetAll()
        {
			return _rooms;
        }
    }
}
