using Xamarin.Forms;

namespace SharedSquawk.Client.Seedwork.Controls
{
    public class MvvmableTabbedPage : TabbedPage
    {
        private readonly ViewModelBase _viewModel;

        public MvvmableTabbedPage(ViewModelBase viewModel)
        {
            _viewModel = viewModel;
            BindingContext = viewModel;
            Title = "SharedSquawk";
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(this);
            base.OnAppearing();
        }
    }
}