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
				FontSize = 20,
				FontAttributes = FontAttributes.Bold
			};
			nameLabel.SetBinding(Label.TextProperty, new Binding("Header"));

			var genderLabel = new Label () {
				FontSize = 14
			};
			genderLabel.SetBinding(Label.TextProperty, new Binding("GenderString"));

			var localeLabel = new Label (){
				FontSize = 14
			};
			localeLabel.SetBinding(Label.TextProperty, new Binding("LocaleDetails"));

			var knownLanguagesLabel = new Label (){
				FontSize = 14
			};
			knownLanguagesLabel.SetBinding(Label.TextProperty, new Binding("KnownLanguagesDisplay"));

			var practicingLanguagesLabel = new Label (){
				FontSize = 14
			};
			practicingLanguagesLabel.SetBinding(Label.TextProperty, new Binding("PracticingLanguagesDisplay"));

			var userPermissionsLabel = new Label (){
				TextColor = Color.Gray,
				FontSize = 12
			};
			userPermissionsLabel.SetBinding(Label.TextProperty, new Binding("UserPermissionsDisplay"));

			var spacer = new BoxView () {
				Color = Color.Transparent,
				HeightRequest = 10
			};

			var descriptionLabel = new Label () {
				FontSize = 14
			};
			descriptionLabel.SetBinding(Label.TextProperty, new Binding("Description"));

			var chatButton = new Button () {
				Text = "Start Chat",
				HeightRequest = 40
			};
			chatButton.SetBinding(Button.CommandProperty, new Binding("SelectChatCommand"));

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
					spacer,
					descriptionLabel
				},
				Padding = 25
			};
			var scrollView = new ScrollView()
			{
				Content = scrollableLayout,
				Padding = new Thickness(5)
			};
			var buttonLayout = new StackLayout {	
				Children = {
					chatButton,
					new BoxView{HeightRequest = 1, Color = Color.Transparent}
				},
				BackgroundColor = Styling.FooterColor
			};
			buttonLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("HasChatOption", BindingMode.OneWay));
			Content = new StackLayout {
				Children = {
					scrollView,
					buttonLayout
				},
				VerticalOptions = LayoutOptions.FillAndExpand
			};
		}
	}
}

