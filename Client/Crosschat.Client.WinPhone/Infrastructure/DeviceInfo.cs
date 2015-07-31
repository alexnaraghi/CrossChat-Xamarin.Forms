using System.Threading.Tasks;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.WinPhone.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInfo))]

namespace SharedSquawk.Client.WinPhone.Infrastructure
{
    public class DeviceInfo : IDeviceInfo
    {
        public Task InitAsync()
        {
            return Task.FromResult(false);
        }

        public string Huid { get { return "TODO:HUID"; }}
        public string PushUri { get; private set; }

		public string Version {
			get 
			{
				return "TODO: version";
			}
		}
    }
}
