using System;
using Xamarin.Forms;
using SharedSquawk.Client;
using SharedSquawk.Client.Droid.CustomRenderers;
using Xamarin.Forms.Platform.Android;
using Android.Content.Res;

[assembly: ExportRenderer (typeof (SquawkEntry), typeof (SquawkEntryRenderer))]
namespace SharedSquawk.Client.Droid.CustomRenderers
{
	public class SquawkEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);
			if (Control != null)
			{
				Control.SetHintTextColor (Styling.HintTextColor.ToAndroid());
				Control.SetBackgroundDrawable (null);
			}
		}
	}
}

