using System;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Seedwork;
using Xamarin.Forms;

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

			// Accomodate iPhone status bar.
			Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 5);

			var scrollableLayout = new StackLayout
			{
				Children =
				{
					nameLabel,
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
			Content = new StackLayout {
				Children = {
					scrollView,
					buttonLayout
				}
			};
		}
	}
}

