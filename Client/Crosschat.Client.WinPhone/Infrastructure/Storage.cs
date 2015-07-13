using System.IO.IsolatedStorage;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.WinPhone.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(Storage))]


namespace SharedSquawk.Client.WinPhone.Infrastructure
{
    public class Storage : IStorage
    {
        public void Set<T>(T obj, string key = "")
        {
            IsolatedStorageSettings.ApplicationSettings[key] = obj;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public void Set<T>(ref T field, T obj, string key = "")
        {
            field = obj;
            Set(obj, key);
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            object value = defaultValue;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value);
            return value is T ? (T)value : defaultValue;
        }
    }
}
