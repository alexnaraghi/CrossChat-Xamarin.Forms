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

			var header = new SquawkLabel
			{
				FormattedText = fs,
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Styling.BlackText
			};

			var loginButton = new Button();
			loginButton.Text = "Login";
			loginButton.SetBinding(IsEnabledProperty, new Binding("IsBusy", converter: new InverterConverter()));
			loginButton.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));
			loginButton.FontSize = 24;

			var registerButton = new Button();
			registerButton.FontAttributes = FontAttributes.Italic;
			registerButton.Text = "Don't have an account?";
			registerButton.SetBinding(Button.CommandProperty, new Binding("RegisterCommand"));
			registerButton.FontSize = 12;

			var nameEntry = new SquawkEntry
			{
				Keyboard = Keyboard.Text,
				Placeholder = "Username",
			};
			nameEntry.SetBinding(Entry.TextProperty, new Binding("Name", BindingMode.TwoWay));

			var passwordEntry = new SquawkEntry
			{
				Keyboard = Keyboard.Text,
				IsPassword = true,
				Placeholder = "Password",
			};
			passwordEntry.SetBinding(Entry.TextProperty, new Binding("Password", BindingMode.TwoWay));


			// Accomodate iPhone status bar.
			Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 100);

			var loadingIndicator = new ActivityIndicator ();
			loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
			loadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

			Content = new StackLayout
			{
				Children =
				{
					header,
					new StackLayout
					{
						Children =
						{
							nameEntry,
							passwordEntry
						}
					},
					loginButton,
					loadingIndicator,
					registerButton
				},
				VerticalOptions = LayoutOptions.Center,
				Spacing = 30,
				Padding = new Thickness(25, 80, 25, 0)
			};
		}
	}
}


