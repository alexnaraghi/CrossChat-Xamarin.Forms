using System;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
	public class SquawkNavigationPage : NavigationPage
	{
		public SquawkNavigationPage (Page root) : base(root)
		{
			BarBackgroundColor = Styling.HeaderYellow;
		}
	}
}

