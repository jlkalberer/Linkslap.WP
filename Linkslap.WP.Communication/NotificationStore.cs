namespace Linkslap.WP.Communication.Notifications
{
    using System;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Windows.Networking.PushNotifications;

    /// <summary>
    /// The notification store.
    /// </summary>
    public class NotificationStore
    {
        /// <summary>
        /// The register.
        /// </summary>
        public async void Register()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/register", registration);
        }

        /// <summary>
        /// The un register.
        /// </summary>
        public async void UnRegister()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/unregister", registration);
        }
    }
}
