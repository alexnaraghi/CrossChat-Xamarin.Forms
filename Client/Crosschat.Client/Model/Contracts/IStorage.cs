using System.Runtime.CompilerServices;

namespace SharedSquawk.Client.Model.Contracts
{
    public interface IStorage
    {
        void Set<T>(T obj, [CallerMemberName] string key = "");

        void Set<T>(ref T field, T obj, [CallerMemberName] string key = "");

        T Get<T>([CallerMemberName] string key = "", T defaultValue = default(T));
    }
}