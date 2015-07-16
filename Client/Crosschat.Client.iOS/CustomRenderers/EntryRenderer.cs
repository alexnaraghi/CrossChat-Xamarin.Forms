using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SharedSquawk.Client;
using SharedSquawk.Client.iOS.CustomRenderers;
using MonoTouch.UIKit;

//Work in progress...
//[assembly: ExportRenderer ( typeof (SquawkEntry), typeof (SquawkEntryRenderer ) ) ]

namespace SharedSquawk.Client.iOS.CustomRenderers
{
	public class SquawkEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged ( ElementChangedEventArgs<Entry> e )
		{
			base.OnElementChanged  (e );

			if ( Control != null ) 
			{   
				Control.TouchUpInside += ( sender, el ) =>
				{
					UIView ctl = Control;
					while ( true )
					{
						ctl = ctl.Superview;
						if ( ctl.Description.Contains ( "UIView" ) )
							break;
					}
					ctl.EndEditing ( true );
				};
			}
		}
	}
}