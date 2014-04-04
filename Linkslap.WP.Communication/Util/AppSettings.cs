namespace Linkslap.WP.Communication.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// The app settings.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Initializes static members of the <see cref="AppSettings"/> class.
        /// </summary>
        static AppSettings()
        {
            var xml = XDocument.Load("settings.xml");

            if (xml == null || xml.Root == null)
            {
                throw new Exception("You must create a settings.xml file.  Use settings-example.xml as reference.");
            }

            var settings = xml.Root.Elements("add").ToList();

            BaseUrl = GetValue("baseUrl", settings);
            HubConnectionString = GetValue("hubConnectionString", settings);
            NotificationHubPath = GetValue("notificationHubPath", settings);
        }

        /// <summary>
        /// Gets the base url.
        /// </summary>
        public static string BaseUrl { get; private set; }

        /// <summary>
        /// Gets the hub connection string.
        /// </summary>
        public static string HubConnectionString { get; private set; }

        /// <summary>
        /// Gets the notification channel.
        /// </summary>
        public static string NotificationHubPath { get; private set; }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="keyName">
        /// The key name.
        /// </param>
        /// <param name="elements">
        /// The elements.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetValue(string keyName, IEnumerable<XElement> elements)
        {
            var element = elements.FirstOrDefault(
                e =>
                    {
                        var attr = e.Attribute("key");
                        return attr != null && attr.Value == keyName;
                    });

            if (element == null)
            {
                return null;
            }

            var valueAttribute = element.Attribute("value");

            if (valueAttribute == null)
            {
                return null;
            }

            return valueAttribute.Value;
        }
    }
}
