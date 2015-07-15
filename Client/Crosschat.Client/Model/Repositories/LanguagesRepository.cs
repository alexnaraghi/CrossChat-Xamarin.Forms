using SharedSquawk.Client.Model.Entities;
using System.Linq;

namespace SharedSquawk.Client.Model
{
	public class LanguagesRepository
	{
		public static string Get(int languageId)
		{
			var item = _languages.Where(w => w.LanguageId == languageId).FirstOrDefault();
			if (item == null)
			{
				return string.Empty;
			}
			else
			{
				return item.Name;
			}
		}

		private static readonly Language[] _languages = 
			new[] {
			new Language (32, "Afrikaans"),
			new Language (53, "Albanian"),
			new Language (61, "Amharic"),
			new Language (10, "Arabic"),
			new Language (20, "Arabic (Egyptian)"),
			new Language (78, "Arabic (Other)"),
			new Language (51, "Armenian"),
			new Language (95, "Assamese"),
			new Language (87, "Azerbaijani"),
			new Language (70, "Basque"),
			new Language (94, "Belorussian"),
			new Language (44, "Bengali"),
			new Language (85, "Berber"),
			new Language (97, "Bichelama"),
			new Language (46, "Bosnian"),
			new Language (96, "Breton"),
			new Language (45, "Bulgarian"),
			new Language (67, "Burmese"),
			new Language (52, "Cambodian"),
			new Language (50, "Catalan"),
			new Language (13, "Chinese (Cantonese)"),
			new Language (7, "Chinese (Mandarin)"),
			new Language (79, "Chinese (Other)"),
			new Language (111, "Corsican"),
			new Language (76, "Creole (Dutch)"),
			new Language (73, "Creole (English)"),
			new Language (72, "Creole (French)"),
			new Language (74, "Creole (German)"),
			new Language (77, "Creole (Portuguese)"),
			new Language (75, "Creole (Spanish)"),
			new Language (41, "Croatian"),
			new Language (38, "Czech"),
			new Language (35, "Danish"),
			new Language (15, "Dutch"),
			new Language (1, "English"),
			new Language (63, "Esperanto"),
			new Language (69, "Estonian"),
			new Language (86, "Fijian"),
			new Language (30, "Finnish"),
			new Language (3, "French"),
			new Language (90, "Galician"),
			new Language (82, "Georgian"),
			new Language (5, "German"),
			new Language (19, "Greek"),
			new Language (98, "Guarani"),
			new Language (49, "Gujarati"),
			new Language (99, "Hakka"),
			new Language (60, "Hawaiian"),
			new Language (29, "Hebrew"),
			new Language (12, "Hindi"),
			new Language (36, "Hungarian"),
			new Language (47, "Icelandic"),
			new Language (34, "Indonesian"),
			new Language (33, "Irish"),
			new Language (6, "Italian"),
			new Language (4, "Japanese"),
			new Language (100, "Javanese"),
			new Language (112, "Kalaallisut"),
			new Language (48, "Kannada"),
			new Language (89, "Kazakh"),
			new Language (11, "Korean"),
			new Language (108, "Kurdish"),
			new Language (101, "Kyrgyz"),
			new Language (102, "Ladino"),
			new Language (109, "Laotian"),
			new Language (59, "Latin"),
			new Language (65, "Latvian"),
			new Language (57, "Lithuanian"),
			new Language (103, "Luxembourgian"),
			new Language (84, "Macedonian"),
			new Language (28, "Malay"),
			new Language (42, "Malayalam"),
			new Language (93, "Maltese"),
			new Language (104, "Manx"),
			new Language (54, "Marathi"),
			new Language (81, "Mongolian"),
			new Language (68, "Nepali"),
			new Language (26, "Norwegian"),
			new Language (88, "Oriya"),
			new Language (113, "Pashto"),
			new Language (107, "Persian (Dari)"),
			new Language (21, "Persian (Farsi)"),
			new Language (23, "Polish"),
			new Language (9, "Portuguese"),
			new Language (40, "Punjabi"),
			new Language (92, "Quechua"),
			new Language (25, "Romanian"),
			new Language (8, "Russian"),
			new Language (58, "Scottish"),
			new Language (31, "Serbian"),
			new Language (56, "Slovak"),
			new Language (62, "Slovenian"),
			new Language (91, "Somali"),
			new Language (2, "Spanish"),
			new Language (43, "Swahili"),
			new Language (18, "Swedish"),
			new Language (14, "Tagalog"),
			new Language (24, "Tamil"),
			new Language (37, "Telugu"),
			new Language (22, "Thaï"),
			new Language (64, "Tibetan"),
			new Language (17, "Turkish"),
			new Language (105, "Turkmen"),
			new Language (55, "Ukrainian"),
			new Language (16, "Urdu"),
			new Language (110, "Uyghur"),
			new Language (71, "Uzbek"),
			new Language (27, "Vietnamese"),
			new Language (83, "Welsh"),
			new Language (106, "Wolof"),
			new Language (66, "Yiddish")

				};

		public static Language[] GetAll ()
		{
			return _languages;
		}
	}
}
