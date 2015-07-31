using System;
using Xamarin.Forms;

namespace SharedSquawk.Client
{
	public static class Styling
	{
		public static Color HeaderYellow {get{return Color.FromHex("#FCED22");}}
		public static Color SubheaderYellow {get{return Color.FromHex("#FEF9B4");}}
		public static Color BackgroundYellow {get{return Color.FromHex("#FCED22");}}
		public static Color EntryBackgroundColor {get{return Device.OnPlatform (iOS: Color.FromHex("#FFFFFF"), Android: Color.FromHex("#FFFFFF"), WinPhone: Color.FromHex("#FFFFFF"));}}
		public static Color FooterColor {get{return Color.FromHex("#f5f5f5");}}
		public static string ChatIcon {get{return "chat.png";}}
		public static string PublicRoomsIcon {get{return "group.png";}}
		public static string NetworkIcon {get{return "group.png";}}
		public static string SettingsIcon {get{return "settings.png";}}
		public static string ToolbarIcon {get{return "popup.png";}}
		public static Color BlackText {get{return Color.FromHex("#000000");}}
		public static Color EntryTextColor {get{return Color.FromHex("#000000");}}
		public static Color HintTextColor {get{return Color.FromHex("#B8B8B8");}}
		public static Color BackgroundColor {get{return Color.White;}}
		public static Color BarColor {get{return Color.FromHex("#5E5E5E");}}

		public static int Sized(int fontSize)
		{
			if (Device.Idiom == TargetIdiom.Tablet)
			{
				return fontSize * 2;
			}
			return fontSize;
		}
	}
}

