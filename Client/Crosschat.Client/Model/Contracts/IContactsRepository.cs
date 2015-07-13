using System.Threading.Tasks;
using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client.Model.Contracts
{
    public interface IContactsRepository
    {
        Task<Contact[]> GetAllAsync();
    }
}
