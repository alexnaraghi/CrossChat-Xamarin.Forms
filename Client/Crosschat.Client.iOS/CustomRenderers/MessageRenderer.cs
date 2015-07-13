using Crosschat.Client.iOS.CustomRenderers;
using Crosschat.Client.ViewModels;
using Crosschat.Client.Views.Controls;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]

namespace Crosschat.Client.iOS.CustomRenderers
{
    public class MessageRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reuseableCell, UITableView tv)
        {
            var textVm = item.BindingContext as TextMessageViewModel;
            if (textVm != null)
            {
				//We know the text is ours if it's on the right, don't print the name out
				string text;
				if (textVm.IsMine)
				{
					text = textVm.Text;
				}
				else
				{
					text = textVm.AuthorName + "· " + textVm.Text;
				}
                var chatBubble = new ChatBubble(!textVm.IsMine, text);
                return chatBubble.GetCell(tv);
            }
			return base.GetCell(item, reuseableCell, tv);
        }
    }
}