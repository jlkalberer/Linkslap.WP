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
            try
            {
                if (Channel == null)
                {
                    Channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                }
            }
            catch (Exception)
            {
                return;
            }

            this.RegisterToUserStreams();
        }

        /// <summary>
        /// The register to user streams.
        /// </summary>
        public void RegisterToUserStreams()
        {
            var registration = new PushRegistration { InstallationId = Storage.GetInstallationId(), ChannelUri = Channel.Uri };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/register", registration);
        }

        /// <summary>
        /// The un register.
        /// </summary>
        public async void UnRegister()
        {
            Channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = Channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/unregister", registration);
        }

        /// <summary>
        /// Gets the channel.
        /// </summary>
        public static PushNotificationChannel Channel { get; private set; }
    }
}
