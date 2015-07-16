using System;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Seedwork;
using Xamarin.Forms;
using SharedSquawk.Client.Views.ValueConverters;

namespace SharedSquawk.Client.Views
{
	public class UserDetailPage : MvvmableContentPage
	{
		public UserDetailPage (ViewModelBase viewModel) : base(viewModel)
		{
			Title = "Details";
			var nameLabel = new Label () {
				FontSize = 16,
				FontAttributes = FontAttributes.Bold
			};
			nameLabel.SetBinding(Label.TextProperty, new Binding("Header"));

			var genderLabel = new Label ();
			genderLabel.SetBinding(Label.TextProperty, new Binding("GenderString"));

			var localeLabel = new Label ();
			localeLabel.SetBinding(Label.TextProperty, new Binding("LocaleDetails"));

			var knownLanguagesLabel = new Label ();
			knownLanguagesLabel.SetBinding(Label.TextProperty, new Binding("KnownLanguagesDisplay"));

			var practicingLanguagesLabel = new Label ();
			practicingLanguagesLabel.SetBinding(Label.TextProperty, new Binding("PracticingLanguagesDisplay"));

			var userPermissionsLabel = new Label ();
			userPermissionsLabel.SetBinding(Label.TextProperty, new Binding("UserPermissionsDisplay"));


			var descriptionLabel = new Label ();
			descriptionLabel.SetBinding(Label.TextProperty, new Binding("Description"));

			var chatButton = new Button () {
				Text = "Start Chat"
			};
			chatButton.SetBinding(Button.CommandProperty, new Binding("SelectChatCommand"));

			// Accomodate iPhone status bar.
			Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 5);

			var scrollableLayout = new StackLayout
			{
				Children =
				{
					nameLabel,
					genderLabel,
					localeLabel,
					knownLanguagesLabel,
					practicingLanguagesLabel,
					userPermissionsLabel,
					descriptionLabel
				}
			};
			var scrollView = new ScrollView()
			{
				Content = scrollableLayout
			};
			var buttonLayout = new StackLayout {	
				Children = {
					chatButton
				}
			};
			buttonLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("HasChatOption", BindingMode.OneWay, converter: new InverterConverter()));
			Content = new StackLayout {
				Children = {
					scrollView,
					buttonLayout
				}
			};
		}
	}
}

