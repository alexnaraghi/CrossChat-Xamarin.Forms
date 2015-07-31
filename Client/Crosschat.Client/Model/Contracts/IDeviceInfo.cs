using System.Threading.Tasks;

namespace SharedSquawk.Client.Model.Contracts
{
    public interface IDeviceInfo
    {
        Task InitAsync();

        string Huid { get; }

        string PushUri { get; }

		string Version { get; }
    }
}