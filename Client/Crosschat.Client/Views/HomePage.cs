﻿using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class HomePage : MvvmableTabbedPage
    {
        public HomePage(ViewModelBase viewModel) : base(viewModel)
        {
			//Children.Add(new ActiveChatPage(viewModel));
			Children.Add(new PublicRoomsPage(viewModel));
            Children.Add(new OnlineUsersPage(viewModel));
			Children.Add(new SettingsPage(viewModel));
        }
    }
}
