using System;
using System.Threading;
using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.DataTransferObjects.Utils;
using Crosschat.Server.Infrastructure.Protocol;
using Crosschat.Utils.Tasking;

namespace Crosschat.Client.Model.Proxies
{
	public class ConnectionManager
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private readonly ITransportResource _transport;
        private readonly RequestsHandler _requestsHandler;
        private readonly IDtoSerializer _serializer;
        private long _lastToken = 1;

        public ConnectionManager(
            ITransportResource transport,
            RequestsHandler requestsHandler,
            IDtoSerializer serializer)
        {
            _transport = transport;
            _requestsHandler = requestsHandler;
            _serializer = serializer;

			_transport.DataReceived += DataReceived;
            _transport.ConnectionError += Transport_ConnectionError;

		}

		private void DataReceived(ResponseBase response)
		{
			_requestsHandler.AppendResponse (response);
		}

        private void Transport_ConnectionError()
        {
            IsConnected = false;
            IsConnecting = false;
            ConnectionDropped();
        }

        public event Action ConnectionDropped = delegate { }; 

        public event EventHandler<DtoEventArgs> DtoReceived = delegate { };

		//AlexTest
        //public event EventHandler<RequestEventArgs> RequestReceived = delegate { };

        public async Task ConnectAsync()
        {
            try
            {
                if (IsConnected)
                    return;
                await _semaphoreSlim.WaitAsync();
                if (IsConnected)
                    return;
                IsConnecting = true;
                await _transport.ConnectAsync();
                IsConnecting = false;
                IsConnected = true;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public bool IsConnecting { get; private set; }

        public bool IsConnected { get; private set; }

		internal Task<TResponse> SendRequestAndWaitResponse<TRequest, TResponse>(TRequest request)
			where TRequest : RequestBase
			where TResponse : ResponseBase
        {
            Interlocked.Increment(ref _lastToken);
            request.Token = _lastToken;
			var endpoint = EndpointFinder.Get (request.GetType ());

			var response = _requestsHandler.WaitForResponse<TResponse>(request, () => _transport.SendData<TRequest, TResponse>(endpoint, request, request.Token));

			if (!response.Result.RequestResult)
			{
				ConnectionDropped ();
			}
			return response;
        }

		/*
        internal void SendRequest(RequestBase request)
        {
            Interlocked.Increment(ref _lastToken);
            request.Token = _lastToken;
            var requestBytes = _serializer.Serialize(request);
			var endpoint = EndpointFinder.Get (request.GetType ());
			_transport.SendData(endpoint, request, request.Token);
        }
        */

        public async Task DisconnectAsync()
        {
            await _semaphoreSlim.WaitAsync();
            await _transport.DisconnectAsync().WrapWithErrorIgnoring();
            IsConnected = false;
            IsConnecting = false;
            _semaphoreSlim.Release();
        }

        public void SendResponse(ResponseBase response)
        {
			//ALEXTEST
			//Do we need to send responses in this application?
			/*
            var requestBytes = _serializer.Serialize(response);
            var command = new Command(CommandNames.Response, requestBytes);
            _transport.SendData(_commandParser.ToBytes(command));
            */
        }
    }

    public class DtoEventArgs : EventArgs
    {
        public DtoEventArgs(BaseDto dto)
        {
            Dto = dto;
        }

        public BaseDto Dto { get; private set; }
    }

    public class RequestEventArgs : EventArgs
    {
        public RequestBase Request { get; set; }

        public RequestEventArgs(RequestBase request)
        {
            Request = request;
        }
    }
}
