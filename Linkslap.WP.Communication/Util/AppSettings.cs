namespace Linkslap.WP.Communication.Util
{
    /// <summary>
    /// The app settings.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Gets the base url.
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                //return "http://localhost:50328/";
                return "https://linkslap.me/";
            }
        }

        /// <summary>
        /// Gets the hub connection string.
        /// </summary>
        public static string HubConnectionString
        {
            get
            {
                return "Endpoint=sb://linkslap.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Dws7ju/tlq79uMvhO0Ab0svCKB47Ita6Kq+u969IP0w=";
            }
        }

        /// <summary>
        /// Gets the notification channel.
        /// </summary>
        public static string NotificationHubPath
        {
            get
            {
                return "linkslap1";
            }
        }
    }
}
