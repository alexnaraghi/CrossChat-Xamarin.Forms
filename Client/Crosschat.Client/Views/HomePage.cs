using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class HomePage : MvvmableTabbedPage
    {
        public HomePage(ViewModelBase viewModel) : base(viewModel)
        {
			Children.Add(new ActiveChatsPage(viewModel){Title = "Conversations", Icon = Styling.ChatIcon});
			Children.Add(new PublicRoomsPage(viewModel){Title = "Public Rooms", Icon = Styling.PublicRoomsIcon});
			Children.Add(new OnlineUsersPage(viewModel){Title = "Members", Icon = Styling.NetworkIcon});
			Children.Add(new SettingsPage(viewModel){Title = "Settings", Icon = Styling.SettingsIcon});
        }
    }
}
