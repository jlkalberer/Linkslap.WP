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
            this.Channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = Channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/register", registration);
        }

        /// <summary>
        /// The un register.
        /// </summary>
        public async void UnRegister()
        {
            this.Channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = this.Channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/unregister", registration);
        }

        public PushNotificationChannel Channel { get; private set; }
    }
}
