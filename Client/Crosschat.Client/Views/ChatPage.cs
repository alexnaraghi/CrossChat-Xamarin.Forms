using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using SharedSquawk.Client.Views.Controls;
using Xamarin.Forms;
using System.Linq;

namespace SharedSquawk.Client.Views
{
    public class ChatPage : MvvmableContentPage
    {
		private ChatListView _messageList;

        public ChatPage(ViewModelBase viewModel) : base(viewModel)
        {
			SetBinding (ContentPage.TitleProperty, new Binding("RoomName"));
            Icon = "chat.png";

            var headerLabel = new Label();
            headerLabel.FontSize = 24;
            headerLabel.TextColor = Device.OnPlatform(Color.Green, Color.Yellow, Color.Yellow);
            headerLabel.SetBinding(Label.TextProperty, new Binding("Subject", stringFormat:"  {0}"));

            var sendButton = new Button();
            sendButton.Text = " Send ";
            sendButton.VerticalOptions = LayoutOptions.EndAndExpand;
            sendButton.SetBinding(Button.CommandProperty, new Binding("SendMessageCommand"));
            if (Device.OS == TargetPlatform.WinPhone)
            {
                sendButton.BackgroundColor = Color.Green;
                sendButton.BorderColor = Color.Green;
                sendButton.TextColor = Color.White; 
            }

            var inputBox = new Entry();
            inputBox.HorizontalOptions = LayoutOptions.FillAndExpand;
            inputBox.Keyboard = Keyboard.Chat;
            inputBox.Placeholder = "Type a message...";
            inputBox.HeightRequest = 30;
            inputBox.SetBinding(Entry.TextProperty, new Binding("InputText", BindingMode.TwoWay));

            _messageList = new ChatListView();
            _messageList.VerticalOptions = LayoutOptions.FillAndExpand;
			_messageList.SetBinding(ChatListView.ItemsSourceProperty, new Binding("MessageEvents"));
            _messageList.ItemTemplate = new DataTemplate(CreateMessageCell);
			_messageList.ItemTapped += ItemTapped;

			var typingLabel = new Label();
			typingLabel.FontSize = 12;
			typingLabel.TextColor = Color.Gray;
			typingLabel.SetBinding(Label.TextProperty, new Binding("TypingEventsString", stringFormat:"  {0}"));
            
            Content = new StackLayout
                {
                    Padding = Device.OnPlatform(new Thickness(6,6,6,6), new Thickness(0), new Thickness(0)),
                    Children =
                        {

							//testButton,
                            
                            //headerLabel,
							_messageList,
							typingLabel,
							new StackLayout
							{
								Children = {inputBox, sendButton},
								Orientation = StackOrientation.Horizontal,
								Padding = new Thickness(0, Device.OnPlatform(0, 20, 0),0,0),
								VerticalOptions = LayoutOptions.End
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
