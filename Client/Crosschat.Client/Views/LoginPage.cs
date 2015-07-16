using System;

using Xamarin.Forms;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Views.ValueConverters;

namespace SharedSquawk.Client.Views
{
	public class LoginPage : MvvmableContentPage
	{
		public LoginPage(ViewModelBase viewModel) : base(viewModel)
		{
			BackgroundColor = Styling.BackgroundYellow;
			var fs = new FormattedString () {
				Spans = {
					new Span{ Text = "Shared", FontSize = 32, FontAttributes = FontAttributes.None },
					new Span{ Text = "Squawk", FontSize = 32, FontAttributes = FontAttributes.Bold },
				}
			};
			var header = new Label
			{
				FormattedText = fs,
				HorizontalOptions = LayoutOptions.Center
			};

			var spacer = new BoxView () {
				Color = Color.Transparent,
				HeightRequest = 40
			};

			var button = new Button();
			button.Text = "Login";
			button.SetBinding(IsEnabledProperty, new Binding("IsBusy", converter: new InverterConverter()));
			button.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));
			button.FontSize = 20;
			//button.BackgroundColor = Color.Gray;
			//button.TextColor = Color.White;

			var nameEntry = new Entry
			{
				Keyboard = Keyboard.Text,
				Placeholder = "Username",
			};
			nameEntry.SetBinding(Entry.TextProperty, new Binding("Name", BindingMode.TwoWay));

			var passwordEntry = new Entry
			{
				Keyboard = Keyboard.Text,
				IsPassword = true,
				Placeholder = "Password",
			};
			passwordEntry.SetBinding(Entry.TextProperty, new Binding("Password", BindingMode.TwoWay));


			// Accomodate iPhone status bar.
			Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 100);

			Content = new StackLayout
			{
				Children =
				{
					header,
					spacer,
					nameEntry,
					passwordEntry,
					button
				},
				VerticalOptions = LayoutOptions.Center
			};
		}
	}
}


