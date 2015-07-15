using System;

namespace SharedSquawk.Client.Model.Entities
{
	public class Locale
	{
		public Locale(int localeId, string name )
		{
			LocaleId = localeId;
			Name = name;
		}

		public int LocaleId { get; private set; }

		public string Name { get; private set; }

		public override string ToString()
		{
			return string.Format("+{0} ({1})", LocaleId, Name);
		}
	}
}

