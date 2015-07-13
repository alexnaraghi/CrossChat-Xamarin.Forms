using System.Threading.Tasks;

namespace SharedSquawk.Client.Model.Contracts
{
    public interface IPhotoPicker
    {
        Task<byte[]> PickPhoto();
    }
}
