using System.IO.IsolatedStorage;

namespace PingdomAlertShared
{
    public class ObjectStorageHelper<T>
    {
        public void Save(T objectToSave)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(CreateKey<T>()))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(CreateKey<T>());
            }
            IsolatedStorageSettings.ApplicationSettings.Add(CreateKey<T>(), objectToSave);
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public T Load()
        {
            T setting = default(T);
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(CreateKey<T>(), out setting))
            {
                return setting;
            }
            return setting;
        }

        private static string CreateKey<TKeyType>()
        {
            return string.Format("Type{0}", typeof(TKeyType).Name);
        }
    }
}
