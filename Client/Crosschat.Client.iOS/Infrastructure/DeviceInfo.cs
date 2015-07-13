using System.Threading.Tasks;
using SharedSquawk.Client.iOS.Infrastructure;
using SharedSquawk.Client.Model.Contracts;
using Xamarin.Forms;

[assembly: Dependency(implementorType: typeof(DeviceInfo))]

namespace SharedSquawk.Client.iOS.Infrastructure
{
    public class DeviceInfo : IDeviceInfo
    {
        public Task InitAsync()
        {
            return Task.FromResult(false);
        }

        public string Huid { get { return "TODO:HUID"; }}
        public string PushUri { get; private set; }
    }
}
