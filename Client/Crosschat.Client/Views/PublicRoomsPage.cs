﻿using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class PublicRoomsPage : MvvmableContentPage
    {
		public PublicRoomsPage(ViewModelBase viewModel) : base(viewModel)
        {
            Title = "Public Rooms";
            Icon = "group.png";

            var listView = new BindableListView
                {
                    ItemTemplate = new DataTemplate(() =>
                        {
							var textCell = new TextCell();
                            textCell.SetBinding(TextCell.TextProperty, new Binding("Name"));
                            //textCell.SetBinding(TextCell.DetailProperty, new Binding("Description"));
                            return textCell;
                        })
                };

            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("PublicRooms"));
            listView.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("SelectRoomCommand"));

            var contactsLoadingIndicator = new ActivityIndicator();
            contactsLoadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));

            Content = new StackLayout
                {
                    Children =
                            {
                                contactsLoadingIndicator,
                                listView
                            }
                };
        }
    }
}