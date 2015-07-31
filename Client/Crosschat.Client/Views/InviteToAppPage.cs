using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class InviteToAppPage : MvvmableContentPage
    {
        public InviteToAppPage(ViewModelBase viewModel) : base(viewModel)
        {
            Title = "Contacts";

			var contactsCountLabel = new SquawkLabel();
            contactsCountLabel.SetBinding(Label.TextProperty, new Binding("Contacts.Count", stringFormat: "{0} contacts."));

			var tipLabel = new SquawkLabel();
            tipLabel.Text = "Select a contact to send an invitation";

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
                            imageCell.SetBinding(TextCell.DetailProperty, new Binding("Number"));
                            return imageCell;
                        }),
                    IsGroupingEnabled = true,
                    GroupDisplayBinding = new Binding("Name"),
                };

            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("GroupedContacts"));
            listView.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("ContactSelectedCommand"));

            var loadingIndicator = new ActivityIndicator();
            loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));

            Content = new StackLayout
                {
                    Children =
                        {
                            loadingIndicator,
                            contactsCountLabel,
                            tipLabel,
                            listView
                        }
                };
        }
    }
}
