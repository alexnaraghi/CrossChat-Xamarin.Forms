using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class OnlineUsersPage : MvvmableContentPage
    {
        public OnlineUsersPage(ViewModelBase viewModel) : base(viewModel)
        {
			var usersCountLabel = new Label () {
			};
            usersCountLabel.SetBinding(Label.TextProperty, new Binding("Users.Count", stringFormat: "     {0} users online"));
            
            var listView = new BindableListView
                {
                    ItemTemplate = new DataTemplate(() =>
                        {
                            var imageCell = new ImageCell
                                {
                                    ImageSource = Device.OnPlatform(
                                        ImageSource.FromFile("empty_contact.jpg"),
                                        ImageSource.FromFile("empty_contact.jpg"),
                                        ImageSource.FromFile("Assets/empty_contact.jpg")),
                                };
                            imageCell.SetBinding(TextCell.TextProperty, new Binding("Name"));
                            imageCell.SetBinding(TextCell.DetailProperty, new Binding("Description"));
                            return imageCell;
                        })
                };

            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("Users"));
			listView.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("SelectUserCommand"));

			var contactsLoadingIndicator = new ActivityIndicator ();
			contactsLoadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
			contactsLoadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

			Content = new StackLayout {
				Children = {
					new StackLayout {Children = 
						{
							usersCountLabel,
							contactsLoadingIndicator
						},
						BackgroundColor = Styling.SubheaderYellow,
						Padding = new Thickness(3)
					},
					listView
				}
			};
		}
	}
}