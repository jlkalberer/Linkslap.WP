namespace Linkslap.WP.Communication.Util
{
    using System.IO.IsolatedStorage;

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

            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(key))
            {
                store.Remove(key);
            }

            store.Save();
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

            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(key))
            {
                store[key] = value;
            }
            else
            {
                store.Add(key, value);
            }

            store.Save();
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
            var store = IsolatedStorageSettings.ApplicationSettings;
            if (!store.Contains(key))
            {
                return default(TModel);
            }

            return (TModel)store[key];
        }
    }
}
