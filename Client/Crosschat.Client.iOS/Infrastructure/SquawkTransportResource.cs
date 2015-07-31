using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SharedSquawk.Client.iOS.Infrastructure;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Server.Infrastructure;
using Xamarin.Forms;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System.Xml;
using System.Reflection;

[assembly: Dependency(typeof(SquawkTransportResource))]

namespace SharedSquawk.Client.iOS.Infrastructure
{
    public class 
		SquawkTransportResource: ITransportResource
    {
        public event Action ConnectionError = delegate { };
		public event Action<ResponseBase> DataReceived = delegate { };
		private DtoSerializer _serializer;

		public SquawkTransportResource()
		{
			_serializer = new DtoSerializer ();	
		}

        public void DropConnection()
        {
            ConnectionError();
        }

		public void SendData<TRequest, TResponse>(TransportEndpoint endpoint, TRequest data, long token) 
			where TRequest : RequestBase 
			where TResponse : ResponseBase
		{
			var request = (HttpWebRequest)WebRequest.Create(GlobalConfig.ServiceBase+endpoint.Address);

			request.Method = endpoint.TransportMethod.ToString();

			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = GlobalConfig.UserAgent;
			request.Referer = GlobalConfig.Referer;

			request.Headers.Add ("X-Requested-With", GlobalConfig.RequestedWith);
			request.Headers.Add (HttpRequestHeader.AcceptEncoding, GlobalConfig.AcceptEncoding);
			//ALEXTEST
			//TODO: Set the language
			request.Headers.Add (HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
			request.Timeout = GlobalConfig.TimeoutMs;

			Stream stream = request.GetRequestStream();
			_serializer.Serialize<TRequest> (data, stream);
			stream.Close();

			try
			{
				request.BeginGetResponse(new AsyncCallback(OnDataReceived<TResponse>), new TokenRequest {Request = request, Token = token});
			}
			catch (Exception)
			{
				DropConnection();
			}
		}
		private void OnDataReceived<T>(IAsyncResult ar) where T : ResponseBase
		{
			HttpWebRequest request;
			HttpWebResponse response;
			long token = 0;
			ResponseBase responseObject;
			try
			{
				var tokenRequest = ar.AsyncState as TokenRequest;
				request = tokenRequest.Request;
				token = tokenRequest.Token;
				response = request.EndGetResponse (ar) as HttpWebResponse;
				responseObject = _serializer.Deserialize<T> (response.GetResponseStream());
			}
			catch(Exception)
			{
				responseObject = Activator.CreateInstance<ResponseBase> ();
				responseObject.Error = CommonErrors.Unknown;
			}

			//Here we are downcasting T to an object, but the token will allow us to figure out the type
			responseObject.Token = token;
			DataReceived(responseObject);
		}
    }


	public class TokenRequest
	{
		public HttpWebRequest Request{ get; set;}
		public long Token{ get; set;}
	}
}
