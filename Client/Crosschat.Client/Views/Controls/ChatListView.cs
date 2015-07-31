using Xamarin.Forms;
using System;

namespace SharedSquawk.Client.Views.Controls
{
    public class ChatListView : ListView
    {
		public Action<int, int, bool> ScrollToRowDelegate { get; set; }

		public void ScrollToRow(int itemIndex, int sectionIndex = 0, bool animated = false)
		{
			if (ScrollToRowDelegate != null)
			{
				ScrollToRowDelegate(itemIndex, sectionIndex, animated);
			}
		}
    }
}
