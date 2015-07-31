using System.ComponentModel;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using SharedSquawk.Client.Droid.CustomRenderers;
using SharedSquawk.Client.ViewModels;
using SharedSquawk.Client.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]

namespace SharedSquawk.Client.Droid.CustomRenderers
{
    public class MessageRenderer : ViewCellRenderer
    {
		//You can ignore 'sectionIndex' and 'animated', i left those because of my IOS implementation
		private void ScrollToRow(ViewGroup parent, int itemIndex, int sectionIndex, bool animated)
		{
			//Get Android's ListView 
			Android.Widget.ListView thisCellsListView = (Android.Widget.ListView)parent;

			thisCellsListView.SetSelection (itemIndex);
		}

        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
			//This needs to go first, Xamarin bug - https://forums.xamarin.com/discussion/35736/possible-bug-in-newer-versions-of-xamarin-forms-listview
			var retval = base.GetCellCore(item, convertView, parent, context);

			//Scrolling
			{
				//Get Android's ListView 
				Android.Widget.ListView thisCellsListView = (Android.Widget.ListView)parent;

				//This CustomListView is a Xamarin.Forms.ListView that has a Custom ListView Renderer
				ChatListView tableParent = (ChatListView)base.ParentView;

				//Wire the Delegate on the first cell creation, just once, and pass Android's ListView Instance
				if (tableParent.ScrollToRowDelegate == null)
				{
					tableParent.ScrollToRowDelegate = (itemIndex, sectionIndex, animated) => {
						ScrollToRow (thisCellsListView, itemIndex, sectionIndex, animated);
					};
				}
			}

			//cell creation
			{
				var inflatorservice = (LayoutInflater)Forms.Context.GetSystemService (Android.Content.Context.LayoutInflaterService);
				var dataContext = item.BindingContext as EventViewModel;

				var textMsgVm = dataContext as TextMessageViewModel;

				if (textMsgVm != null)
				{
					var template = (LinearLayout)inflatorservice.Inflate (textMsgVm.IsMine ? Resource.Layout.message_item_owner : Resource.Layout.message_item_opponent, null, false);
					//template.FindViewById<TextView>(Resource.Id.timestamp).Text = textMsgVm.Timestamp.ToString("HH:mm");
					template.FindViewById<TextView> (Resource.Id.nick).Text = textMsgVm.IsMine ? "Me" : textMsgVm.AuthorName;
					template.FindViewById<TextView> (Resource.Id.message).Text = textMsgVm.Text;
					return template;
				}
				return retval;
			}
        }
    }
}