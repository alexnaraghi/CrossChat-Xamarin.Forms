using System;

using Xamarin.Forms;
using Crosschat.Client.Seedwork.Controls;
using Crosschat.Client.Seedwork;
using Crosschat.Client.Views.ValueConverters;

namespace Crosschat.Client
{
	public class LoginPage : MvvmableContentPage
	{
		public LoginPage(ViewModelBase viewModel) : base(viewModel)
		{
			this.BackgroundColor = Color.Yellow;
			var header = new Label
			{
				Text = "Login",
				Font = Font.BoldSystemFontOfSize(36),
				HorizontalOptions = LayoutOptions.Center
			};

			var button = new Button();
			button.Text = "Login";
			button.SetBinding(IsEnabledProperty, new Binding("IsBusy", converter: new InverterConverter()));
			button.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));
			button.BackgroundColor = Color.Gray;
			button.TextColor = Color.White;

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
			Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 5);

			Content = new StackLayout
			{
				Children =
				{
					header,
					nameEntry,
					passwordEntry,
					button
				}
			};
		}
	}
}


