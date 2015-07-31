using Android.App;
using Android.Content.PM;
using Android.OS;

using Xamarin.Forms.Platform.Android;

namespace SharedSquawk.Client.Droid
{
	[Activity(Label = "SharedSquawk", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/CustomTheme")]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);
            SetPage(App.GetMainPage());
        }
    }
}

