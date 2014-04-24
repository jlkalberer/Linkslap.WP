namespace Linkslap.WP.Communication.Util
{
    using Newtonsoft.Json;

    /// <summary>
    /// The storage.
    /// </summary>
    public static class Storage
    {
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

            var store = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
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

            var store = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
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
            var store = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            if (!store.ContainsKey(key))
            {
                return default(TModel);
            }
            var v = (string)store[key];
            return JsonConvert.DeserializeObject<TModel>((string)store[key]);
        }
    }
}
