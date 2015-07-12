using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class SettingsPage : MvvmableContentPage
    {
        public SettingsPage(ViewModelBase viewModel) : base(viewModel)
        {
            Title = "Settings";
            Icon = "settings.png";

            var inviteButton = new Button();
            inviteButton.Text = "Invite contacts";
            inviteButton.SetBinding(Button.CommandProperty, new Binding("InviteCommand"));

			#if DEBUG
			var userLabel = new Label();
			userLabel.SetBinding(Label.TextProperty, new Binding("User"));
			var ssidLabel = new Label();
			ssidLabel.SetBinding(Label.TextProperty, new Binding("SSID"));
			#endif

            Content = new StackLayout
            {
                Children =
                {
                    inviteButton
#if DEBUG
					,userLabel
					,ssidLabel
#endif
                }
            };
        }
    }
}
