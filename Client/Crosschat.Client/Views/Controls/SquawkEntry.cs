using System;
using Xamarin.Forms;

namespace SharedSquawk.Client
{
	public class SquawkEntry : Entry
	{
		public SquawkEntry () : base()
		{
			BackgroundColor = Styling.EntryBackgroundColor;
			TextColor = Styling.EntryTextColor;
		}
	}
}

