using Crosschat.Client.ViewModels;
using Crosschat.Client.Views;
using Xamarin.Forms;
using Crosschat.Client.Model.Managers;
using Crosschat.Client.Model.Contracts;
using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Client
{
    public class App
    {
        public static Page GetMainPage()
        {
			ApplicationManager applicationManager = new ApplicationManager(
				DependencyService.Get<ITransportResource>(),
				DependencyService.Get<IDtoSerializer>(),
				DependencyService.Get<IStorage>(),
				DependencyService.Get<IDeviceInfo>());

			return new NavigationPage(new LoginPage(new LoginViewModel(applicationManager)));
        }
    }
}
