using System;
using System.Threading.Tasks;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;

namespace SharedSquawk.Client.Model.Contracts
{
    public interface ITransportResource
    {
		event Action<ResponseBase> DataReceived;

		void SendData<TRequest, TResponse>(TransportEndpoint endpoint, TRequest data, long token) 
			where TRequest : RequestBase
			where TResponse : ResponseBase;
    }

	public enum TransportMethod
	{
		POST,
		GET
	}

	public struct TransportEndpoint
	{
		public TransportMethod TransportMethod;
		public string Address;
	}
}