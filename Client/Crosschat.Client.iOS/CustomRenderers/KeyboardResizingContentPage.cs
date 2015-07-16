using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SharedSquawk.Client.Seedwork.Controls;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SharedSquawk.Client.Views;

[assembly: ExportRenderer (typeof(ChatPage), typeof(SharedSquawk.Client.iOS.Renderers.IosPageRenderer))]

namespace SharedSquawk.Client.iOS.Renderers
{
	public class IosPageRenderer : PageRenderer
	{
		NSObject observerHideKeyboard;
		NSObject observerShowKeyboard;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
			observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver (observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver (observerShowKeyboard);
		}

		void OnKeyboardNotification (NSNotification notification)
		{
			if (!IsViewLoaded)
				return;

			var frameBegin = UIKeyboard.FrameBeginFromNotification (notification);
			var frameEnd = UIKeyboard.FrameEndFromNotification (notification);

			var page = Element as ChatPage;
			if (page != null && !(page.Content is ScrollView))
			{
				var padding = page.Padding;
				page.Padding = new Thickness (padding.Left, padding.Top, padding.Right, padding.Bottom + frameBegin.Top - frameEnd.Top);
					
			}
		}
	}
}