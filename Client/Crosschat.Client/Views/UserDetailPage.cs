using System;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Seedwork;
using Xamarin.Forms;
using SharedSquawk.Client.Views.ValueConverters;

namespace SharedSquawk.Client.Views
{
	public class UserDetailPage : MvvmableContentPage
	{
		public UserDetailPage (ViewModelBase viewModel) : base (viewModel)
		{
			Title = "Details";
			var nameLabel = new SquawkLabel () {
				FontSize = Styling.Sized (20),
				FontAttributes = FontAttributes.Bold
			};
			nameLabel.SetBinding (SquawkLabel.TextProperty, new Binding ("Header"));

			var genderLabel = new SquawkLabel () {
				FontSize = Styling.Sized (14)
			};
			genderLabel.SetBinding (SquawkLabel.TextProperty, new Binding ("GenderString"));

			var localeLabel = new SquawkLabel () {
				FontSize = Styling.Sized (14)
			};
			localeLabel.SetBinding (SquawkLabel.TextProperty, new Binding ("LocaleDetails"));

			var knownLanguagesLabel = new SquawkLabel () {
				FontSize = Styling.Sized (14)
			};
			knownLanguagesLabel.SetBinding (Label.TextProperty, new Binding ("KnownLanguagesDisplay"));

			var practicingLanguagesLabel = new SquawkLabel () {
				FontSize = Styling.Sized (14)
			};
			practicingLanguagesLabel.SetBinding (SquawkLabel.TextProperty, new Binding ("PracticingLanguagesDisplay"));

			var userPermissionsLabel = new SquawkLabel () {
				TextColor = Color.Gray,
				FontSize = Styling.Sized (12)
			};
			userPermissionsLabel.SetBinding (SquawkLabel.TextProperty, new Binding ("UserPermissionsDisplay"));

			var spacer = new BoxView () {
				Color = Color.Transparent,
				HeightRequest = Styling.Sized (10)
			};

			var descriptionLabel = new SquawkLabel () {
				FontSize = Styling.Sized (14)
			};
			descriptionLabel.SetBinding (SquawkLabel.TextProperty, new Binding ("Description"));

			var chatButton = new Button () {
				Text = "Start Chat",
				HeightRequest = 40,
				FontSize = Styling.Sized (14),
				VerticalOptions = LayoutOptions.Center
			};
			chatButton.SetBinding (Button.CommandProperty, new Binding ("SelectChatCommand"));

			var scrollableLayout = new StackLayout {
				Children = {
					nameLabel,
					genderLabel,
					localeLabel,
					knownLanguagesLabel,
					practicingLanguagesLabel,
					userPermissionsLabel,
					spacer,
					descriptionLabel
				},
				Padding = 25
			};
			var scrollView = new ScrollView () {
				Content = scrollableLayout,
				Padding = new Thickness (5)
			};
			var buttonLayout = new StackLayout {	
				Children = {
					chatButton,
					new BoxView{ HeightRequest = Device.OnPlatform(iOS: 1, Android: 0, WinPhone: 0), Color = Color.Transparent }
				},
				BackgroundColor = Styling.FooterColor,
				Padding = new Thickness(5, 0)
			};
			buttonLayout.SetBinding (StackLayout.IsVisibleProperty, new Binding ("HasChatOption", BindingMode.OneWay));

			Grid grid = new Grid () {
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = 
				{
					new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = GridLength.Auto }
				}
			};
			grid.Children.Add (scrollView, 0, 0);
			grid.Children.Add (buttonLayout, 0, 1);

			Content = grid;

		}
	}
}

