using System;

namespace SharedSquawk.Client.Model.Entities
{
	public class Language
	{
		public Language(int languageId, string name )
		{
			LanguageId = languageId;
			Name = name;
		}

		public int LanguageId { get; private set; }

		public string Name { get; private set; }

		public override string ToString()
		{
			return string.Format("+{0} ({1})", LanguageId, Name);
		}
	}
}

