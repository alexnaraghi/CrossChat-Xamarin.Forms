using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class HomePage : MvvmableTabbedPage
    {
        public HomePage(ViewModelBase viewModel) : base(viewModel)
        {
			Children.Add(new ActiveChatsPage(viewModel){Title = "Conversations", Icon = "chat.png"});
			Children.Add(new PublicRoomsPage(viewModel){Title = "Public Rooms", Icon = "group.png"});
			Children.Add(new OnlineUsersPage(viewModel){Title = "Members", Icon = "group.png"});
			Children.Add(new SettingsPage(viewModel){Title = "Settings", Icon = "settings.png"});
        }
    }
}
