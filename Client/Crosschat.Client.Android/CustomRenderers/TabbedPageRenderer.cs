using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Droid;
using Android.App;
using Android.Views;
using Android.Graphics.Drawables;
using System.ComponentModel;

[assembly: ExportRenderer (typeof (MvvmableTabbedPage), typeof (TabbedPageRenderer))]
namespace SharedSquawk.Client.Droid
{
	public class TabbedPageRenderer : TabbedRenderer
	{
		private bool isFirstTime = true;
		private Activity activity;

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			activity = this.Context as Activity;
		}

		protected override void OnWindowVisibilityChanged(ViewStates visibility)
		{
			base.OnWindowVisibilityChanged(visibility);
			if (isFirstTime)
			{
				ActionBar actionBar = activity.ActionBar;

				ColorDrawable colorDrawable = new ColorDrawable(Styling.BarColor.ToAndroid());
				actionBar.SetStackedBackgroundDrawable(colorDrawable);
				isFirstTime = false; 
			}
		}

	}
}

