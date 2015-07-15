using System.IO;

namespace SharedSquawk.Server.Infrastructure.Protocol
{
    public interface IDtoSerializer
    {
		void Serialize<T> (T data, Stream stream);

		T Deserialize<T>(Stream stream);
    }
}