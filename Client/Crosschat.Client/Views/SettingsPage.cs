using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class SettingsPage : MvvmableContentPage
    {
        public SettingsPage(ViewModelBase viewModel) : base(viewModel)
        {
			var userLabel = new Label();
			userLabel.SetBinding(Label.TextProperty, new Binding("User"));
			userLabel.HorizontalOptions = LayoutOptions.Center;


			var profileButton = new Button();
			profileButton.Text = "Profile";
			profileButton.SetBinding(Button.CommandProperty, new Binding("ViewProfileCommand"));

            var logoutButton = new Button();
			logoutButton.Text = "Logout";
			logoutButton.SetBinding(Button.CommandProperty, new Binding("LogoutCommand"));

			#if DEBUG
			var ssidLabel = new Label();
			ssidLabel.SetBinding(Label.TextProperty, new Binding("SSID"));
			ssidLabel.HorizontalOptions = LayoutOptions.Center;
			#endif

			// Accomodate iPhone status bar.
			Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 5);

            Content = new StackLayout
            {
                Children =
				{
					userLabel,
					profileButton,
					logoutButton
#if DEBUG
					,ssidLabel
#endif
                }
            };
        }
    }
}
