using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class SettingsPage : MvvmableContentPage
    {
        public SettingsPage(ViewModelBase viewModel) : base(viewModel)
        {
			var aboutLabel = new Label()
			{
				HorizontalOptions = LayoutOptions.Center,
				FontSize = 14,
				Text = "SharedSquawk is an app to practice your language skills and meet users " +
					"all over the world.  Talk with people from the www.SharedTalk.com community.  " +
					"\n\nThis app has no affiliation with Rosetta Stone or the www.SharedTalk.com website.\n\n" +
					"It is open source and can be contributed to at www.github.com/alexnaraghi/SharedSquawk",
				TextColor = Color.Gray
			};

			var separator = new BoxView () {
				Color = Color.Gray,
				HeightRequest = 1
			};

			var userLabel = new Label()
			{
				HorizontalOptions = LayoutOptions.Center,
				FontSize = 18
			};
			userLabel.SetBinding(Label.TextProperty, new Binding("User"));


			var profileButton = new Button();
			profileButton.Text = "Profile";
			profileButton.SetBinding(Button.CommandProperty, new Binding("ViewProfileCommand"));

            var logoutButton = new Button();
			logoutButton.Text = "Logout";
			logoutButton.SetBinding(Button.CommandProperty, new Binding("LogoutCommand"));

			// Accomodate iPhone status bar.
			Padding = new Thickness (25, Device.OnPlatform (iOS: 5, Android: 0, WinPhone: 0), 25, 5);

			Content = new StackLayout {
				Children = {
					aboutLabel,
					separator,
					userLabel,
					new StackLayout {
						Children = {
							profileButton,
							logoutButton
						},
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Spacing = 15
					}
				},
				Spacing = 10,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
		}
    }
}
