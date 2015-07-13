using System.Threading.Tasks;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;

namespace SharedSquawk.Client.Model.Managers
{
    public class SearchManager : ManagerBase
    {
        private readonly UsersSearchServiceProxy _searchServiceProxy;

        public SearchManager(ConnectionManager connectionManager, UsersSearchServiceProxy searchServiceProxy)
            : base(connectionManager)
        {
            _searchServiceProxy = searchServiceProxy;
        }

        public async Task<UserDto[]> SearchAsync(string query)
        {
            UsersSearchResponse response = await _searchServiceProxy.SearchUser(new UsersSearchRequest { QueryString = query });
            if (response.Result == null)
                return new UserDto[0];

            return response.Result;
        }
    }
}
