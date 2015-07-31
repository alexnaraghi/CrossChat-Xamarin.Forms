using SharedSquawk.Client.ViewModels;
using SharedSquawk.Client.Views;
using Xamarin.Forms;
using SharedSquawk.Client.Model.Managers;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Server.Infrastructure.Protocol;

namespace SharedSquawk.Client
{
    public class App
    {
        public static Page GetMainPage()
        {
			ApplicationManager applicationManager = new ApplicationManager(
				DependencyService.Get<ITransportResource>(),
				DependencyService.Get<IStorage>(),
				DependencyService.Get<IDeviceInfo>());

			return new LoginPage(new LoginViewModel(applicationManager));
        }
    }
}
