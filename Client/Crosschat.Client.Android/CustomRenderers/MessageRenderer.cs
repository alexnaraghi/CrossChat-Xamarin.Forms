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
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
			//This needs to go first, Xamarin bug - https://forums.xamarin.com/discussion/35736/possible-bug-in-newer-versions-of-xamarin-forms-listview
			var retval = base.GetCellCore(item, convertView, parent, context);

            var inflatorservice = (LayoutInflater)Forms.Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
            var dataContext = item.BindingContext as EventViewModel;

            var textMsgVm = dataContext as TextMessageViewModel;

            if (textMsgVm != null)
            {
				var template = (LinearLayout)inflatorservice.Inflate(textMsgVm.IsMine ? Resource.Layout.message_item_owner : Resource.Layout.message_item_opponent, null, false);
				//template.FindViewById<TextView>(Resource.Id.timestamp).Text = textMsgVm.Timestamp.ToString("HH:mm");
				template.FindViewById<TextView>(Resource.Id.nick).Text = textMsgVm.IsMine ? "Me" : textMsgVm.AuthorName;
				template.FindViewById<TextView>(Resource.Id.message).Text = textMsgVm.Text;
				return template;
            }
			return retval;
        }
    }
}