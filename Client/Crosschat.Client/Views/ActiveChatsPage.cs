using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Controls;
using Xamarin.Forms;
using SharedSquawk.Client.Views.ValueConverters;

namespace SharedSquawk.Client.Views
{
    public class ActiveChatsPage : MvvmableContentPage
    {
		public ActiveChatsPage(ViewModelBase viewModel) : base(viewModel)
		{
			var listView = new BindableListView
			{
				ItemTemplate = new DataTemplate(() =>
					{
						var imageCell = new ImageCell();
						imageCell.SetBinding(ImageCell.TextProperty, new Binding("Name"));
						imageCell.SetBinding(ImageCell.DetailProperty, new Binding("DescriptionText"));
						imageCell.SetBinding(ImageCell.ImageSourceProperty, new Binding("Image"));
						imageCell.TextColor = Styling.CellTitleColor;
						imageCell.DetailColor = Styling.CellDetailColor;
						return imageCell;
					}),
				SeparatorVisibility = SeparatorVisibility.None
			};

			listView.SetBinding(ListView.ItemsSourceProperty, new Binding("ActiveChats"));
			listView.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("SelectActiveChatCommand"));
			listView.SetBinding(ListView.IsVisibleProperty, new Binding("HasConversations", BindingMode.OneWay));
				
			var noItemsLabel = new Label {
				Text = "Start a conversation or open a room!",
				HorizontalOptions = LayoutOptions.Center,
				FontSize = 16,
				TextColor = Color.Gray
			};
			var noItemsLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = 
				{
					new BoxView{HeightRequest = 20},
					noItemsLabel
				}
			};
			noItemsLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("HasConversations", BindingMode.OneWay, converter: new InverterConverter()));
			

			var loadingIndicator = new ActivityIndicator ();
			loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
			loadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

			Content = new StackLayout
			{
				Children =
				{
					loadingIndicator,
					listView,
					noItemsLayout
				}
			};
		}
    }
}