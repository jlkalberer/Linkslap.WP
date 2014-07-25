namespace Linkslap.WP.Communication.Util
{
    using System;

    using Newtonsoft.Json;

    using Windows.Storage;

    /// <summary>
    /// The storage.
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// The get installation id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetInstallationId()
        {
            var container = GetLocalStorageContainer();
            if (!container.Values.ContainsKey("InstallationId"))
            {
                container.Values["InstallationId"] = Guid.NewGuid().ToString();
            }

            return (string)container.Values["InstallationId"];
        }

        public static void ClearAll()
        {
            var store = ApplicationData.Current.LocalSettings.Values;
            store.Clear();
        }

        /// <summary>
        /// The clear persistent.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Clear(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var store = ApplicationData.Current.LocalSettings.Values;
            if (store.ContainsKey(key))
            {
                store.Remove(key);
            }

            return true;
        }

        /// <summary>
        /// The save persistent.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Save(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var store = ApplicationData.Current.LocalSettings.Values;
            var json = JsonConvert.SerializeObject(value);
            if (store.ContainsKey(key))
            {
                store[key] = json;
            }
            else
            {
                store.Add(key, json);
            }

            return true;
        }

        /// <summary>
        /// The load persistent.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of model to load from storage.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TModel"/>.
        /// </returns>
        public static TModel Load<TModel>(string key)
        {
            var store = ApplicationData.Current.LocalSettings.Values;
            if (!store.ContainsKey(key))
            {
                return default(TModel);
            }

            return JsonConvert.DeserializeObject<TModel>((string)store[key]);
        }

        /// <summary>
        /// The get local storage container.
        /// </summary>
        /// <returns>
        /// The <see cref="ApplicationDataContainer"/>.
        /// </returns>
        private static ApplicationDataContainer GetLocalStorageContainer()
        {
            if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey("InstallationContainer"))
            {
                ApplicationData.Current.LocalSettings.CreateContainer(
                    "InstallationContainer",
                    ApplicationDataCreateDisposition.Always);
            }

            return ApplicationData.Current.LocalSettings.Containers["InstallationContainer"];
        }

    }
}
