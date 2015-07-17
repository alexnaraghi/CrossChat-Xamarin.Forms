using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Views.Controls;
using Xamarin.Forms;
using System.Linq;
using SharedSquawk.Client.Views.ValueConverters;

namespace SharedSquawk.Client.Views
{
    public class ChatPage : MvvmableContentPage
    {
		private ChatListView _messageList;

        public ChatPage(ViewModelBase viewModel) : base(viewModel)
        {
			SetBinding (ContentPage.TitleProperty, new Binding("RoomName"));
			Icon = Styling.ChatIcon;

			var toolbarItem = new ToolbarItem {
				Text = "Actions",
				Icon = Styling.ToolbarIcon,
				Order = ToolbarItemOrder.Primary
			};
			toolbarItem.SetBinding(ToolbarItem.CommandProperty, new Binding("ContextOptionsCommand")); 
			ToolbarItems.Add(toolbarItem);

            var headerLabel = new Label();
			headerLabel.FontSize = Styling.Sized(24);
            headerLabel.TextColor = Device.OnPlatform(Color.Green, Color.Yellow, Color.Yellow);
            headerLabel.SetBinding(Label.TextProperty, new Binding("Subject", stringFormat:"  {0}"));

            var sendButton = new Button();
            sendButton.Text = " Send ";
            sendButton.VerticalOptions = LayoutOptions.EndAndExpand;
            sendButton.SetBinding(Button.CommandProperty, new Binding("SendMessageCommand"));
			sendButton.SetBinding(Button.IsEnabledProperty, new Binding("IsInConnectedMode", BindingMode.OneWay));
            if (Device.OS == TargetPlatform.WinPhone)
            {
                sendButton.BackgroundColor = Color.Green;
                sendButton.BorderColor = Color.Green;
                sendButton.TextColor = Color.White; 
            }

			var inputBox = new SquawkEntry();
            inputBox.HorizontalOptions = LayoutOptions.FillAndExpand;
            inputBox.Keyboard = Keyboard.Chat;
            inputBox.Placeholder = "Type a message...";
            inputBox.HeightRequest = 30;
            inputBox.SetBinding(Entry.TextProperty, new Binding("InputText", BindingMode.TwoWay));
			inputBox.SetBinding(Entry.IsEnabledProperty, new Binding("IsInConnectedMode", BindingMode.OneWay));

            _messageList = new ChatListView();
			_messageList.VerticalOptions = LayoutOptions.FillAndExpand;
			_messageList.SetBinding(ChatListView.ItemsSourceProperty, new Binding("MessageEvents"));
            _messageList.ItemTemplate = new DataTemplate(CreateMessageCell);
			_messageList.ItemTapped += ItemTapped;
			_messageList.HasUnevenRows = true;
			_messageList.SeparatorVisibility = SeparatorVisibility.None;
			_messageList.SetBinding(Entry.IsEnabledProperty, new Binding("IsInRequestMode", BindingMode.OneWay, converter: new InverterConverter()));

			var typingLabel = new Label();
			typingLabel.FontSize = Styling.Sized(11);
			typingLabel.TextColor = Color.Gray;
			typingLabel.SetBinding(Label.TextProperty, new Binding("TypingEventsString", stringFormat:"  {0}"));
			//typingLabel.SetBinding(Label.IsVisibleProperty, new Binding("AreTypingEvents"));

			#region Error Message UI
			var chatStatusLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				FontSize = 16,
				TextColor = Color.Gray
			};

			chatStatusLabel.SetBinding(Label.TextProperty, new Binding("StatusText", BindingMode.OneWay));
			var chatStatusLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = 
				{
					chatStatusLabel
				}
			};
			chatStatusLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsInMessageMode", BindingMode.OneWay));
			#endregion

			#region Request UI
			var requestLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				FontSize = Styling.Sized(16),
				Text = "This user wants to chat!  What would you like to do?"
			};

			var acceptButton = new Button(){
				FontSize = Styling.Sized(14)
			};
			acceptButton.Text = "Accept";
			acceptButton.SetBinding(Button.CommandProperty, new Binding("AcceptChatCommand"));
			if (Device.OS == TargetPlatform.WinPhone)
			{
				acceptButton.BackgroundColor = Color.Green;
				acceptButton.BorderColor = Color.Green;
				acceptButton.TextColor = Color.White; 
			}

			var declineButton = new Button(){
				FontSize = Styling.Sized(14)
			};
			declineButton.Text = "Decline";
			declineButton.SetBinding(Button.CommandProperty, new Binding("DeclineChatCommand"));
			if (Device.OS == TargetPlatform.WinPhone)
			{
				declineButton.BackgroundColor = Color.Green;
				declineButton.BorderColor = Color.Green;
				declineButton.TextColor = Color.White; 
			}

			var ignoreButton = new Button();
			{
				ignoreButton.FontSize = Styling.Sized(14);
				ignoreButton.Text = "Ignore";
				ignoreButton.SetBinding(Button.CommandProperty, new Binding("LeaveRoomCommand"));
				if (Device.OS == TargetPlatform.WinPhone)
				{
					ignoreButton.BackgroundColor = Color.Green;
					ignoreButton.BorderColor = Color.Green;
					ignoreButton.TextColor = Color.White; 
				}
			}

			var requestLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Spacing = 15,
				Children = 
				{
					requestLabel,
					acceptButton,
					declineButton,
					ignoreButton
				}
			};
			requestLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsInRequestMode", BindingMode.OneWay));
			#endregion

			Content  = new StackLayout
                {
                    Padding = Device.OnPlatform(new Thickness(6,6,6,6), new Thickness(0), new Thickness(0)),
                    Children =
                        {
							_messageList,
							chatStatusLayout,
							requestLayout,
							typingLabel,
							new StackLayout
							{
						
								Children = {inputBox, sendButton},
								Orientation = StackOrientation.Horizontal,
								Padding = new Thickness(0, Device.OnPlatform(0, 20, 0),0,0),
								//VerticalOptions = LayoutOptions.End
							}
                        }
					};
        }

        void ItemTapped (object sender, ItemTappedEventArgs e)
        {
			// don't do anything if we just de-selected the row
			if (e.Item == null) return; 
			// do something with e.SelectedItem
			((ListView)sender).SelectedItem = null; // de-select the row
        }

		public void OnItemAdded(object lastItem)
		{
			if (lastItem != null)
			{
				_messageList.ScrollTo (lastItem, ScrollToPosition.End, animated: false);
			}
		}

        private Cell CreateMessageCell()
        {
            var timestampLabel = new Label();
            timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
            timestampLabel.TextColor = Color.Silver;
			timestampLabel.FontSize = 14;

            var authorLabel = new Label();
            authorLabel.SetBinding(Label.TextProperty, new Binding("AuthorName", stringFormat: "{0}: "));
            authorLabel.TextColor = Device.OnPlatform(Color.Blue, Color.Yellow, Color.Yellow);
			authorLabel.FontSize = 14;

            var messageLabel = new Label();
            messageLabel.SetBinding(Label.TextProperty, new Binding("Text"));
			messageLabel.FontSize = 14;

            var stack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = {authorLabel, messageLabel}
                };

            if (Device.Idiom == TargetIdiom.Tablet)
            {
				stack.Children.Insert(0, timestampLabel);
            }

            var view = new MessageViewCell
                {
                    View = stack
                };
            return view;
        }
    }
}
