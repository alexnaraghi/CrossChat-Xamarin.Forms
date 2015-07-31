using System.Threading.Tasks;
using SharedSquawk.Client.Droid.Infrastructure;
using SharedSquawk.Client.Model.Contracts;
using Xamarin.Forms;
using Android.Content;

[assembly: Dependency(typeof(DeviceInfo))]

namespace SharedSquawk.Client.Droid.Infrastructure
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
				Context context = Forms.Context;
				return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
			}
		}
    }
}
