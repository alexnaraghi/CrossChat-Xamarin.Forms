using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class OnlineUsersPage : MvvmableContentPage
    {
        public OnlineUsersPage(ViewModelBase viewModel) : base(viewModel)
        {
			var usersCountLabel = new SquawkLabel () {
			};
            usersCountLabel.SetBinding(Label.TextProperty, new Binding("Users.Count", stringFormat: "     {0} users online"));
            
			var filterEntry = new SquawkEntry () {
				Placeholder = "Filter...",
			};
			filterEntry.SetBinding (Entry.TextProperty, new Binding ("FilterText"));

            var listView = new BindableListView
                {
                    ItemTemplate = new DataTemplate(() =>
                        {
                            var imageCell = new ImageCell
                                {
									ImageSource = Styling.ContactIcon
                                };
                            imageCell.SetBinding(TextCell.TextProperty, new Binding("Name"));
                            imageCell.SetBinding(TextCell.DetailProperty, new Binding("Description"));
							imageCell.TextColor = Styling.CellTitleColor;
							imageCell.DetailColor = Styling.CellDetailColor;
                            return imageCell;
                        })
                };

			listView.SetBinding(ListView.ItemsSourceProperty, new Binding("UsersDisplay"));
			listView.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("SelectUserCommand"));

			var loadingIndicator = new ActivityIndicator ();
			loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
			loadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

			Content = new StackLayout {
				Children = {
					new StackLayout {Children = 
						{
							usersCountLabel,
							filterEntry,
							loadingIndicator
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