using System.Threading.Tasks;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Managers;
using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.ViewModels;
using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Infrastructure.Protocol;
using Xamarin.Forms;

namespace SharedSquawk.Client.Views
{
    public class SplashscreenPage : ContentPage
    {
		public SplashscreenPage()
        {
            Title = "";

            Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                        {
                            new Label
                                {
                                    Text = "Connecting...", 
                                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                                    FontSize = 24,
                                },
                            new ActivityIndicator 
                                {
                                    IsRunning = true, 
                                }
                        }
                };
            
            //NOTE: this button is a workaround, adding button on HomePage doesn't work so it will be presented always
            //but will work only in Chat
            var sendImageItem = new ToolbarItem("send photo", Device.OnPlatform(null, null, "appbar.image.beach.png"),
                () =>
                {
                    var homeVm = ViewModelBase.CurrentViewModel as HomeViewModel;
                    if (homeVm != null)
                    {
                        //homeVm.SendImageCommand.Execute(null);
                    }
                    else if (ViewModelBase.CurrentViewModel != null)
                    {
                        ViewModelBase.CurrentViewModel.Notify(";(", "You can send images only in chat. I just don't know how to show it only on specific pages - ToolbarItems.Add doesn't work on HomePage ;(");
                    }
                });
            sendImageItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SendImageCommand"));
            Device.OnPlatform(WinPhone: () => ToolbarItems.Add(sendImageItem)); 

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
        }

        protected override async void OnAppearing()
        {
			//ALEXTEST
			//await Navigation.PushAsync(new LoginPage(new LoginViewModel(_applicationManager)));

			/*
			if(true)
			//if (result == AuthenticationResponseType.Success)
            {
                await Navigation.PushAsync(new HomePage(new HomeViewModel(_applicationManager)));
            }
            else
            {
                await Navigation.PushAsync(new RegistrationPage(new RegistrationViewModel(_applicationManager)));
            }
			*/

            base.OnAppearing();
        }

    }
}
