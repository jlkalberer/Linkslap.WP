namespace Linkslap.WP.Communication.Notifications
{
    using System;
    using System.Threading.Tasks;

    using Windows.Foundation;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Windows.Networking.PushNotifications;

    /// <summary>
    /// The notification store.
    /// </summary>
    public class NotificationStore
    {
        /// <summary>
        /// Gets the channel.
        /// </summary>
        private static PushNotificationChannel channel;

        /// <summary>
        /// The push notification received.
        /// </summary>
        public static event TypedEventHandler<PushNotificationChannel, PushNotificationReceivedEventArgs> PushNotificationReceived;

        private static void OnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            TypedEventHandler<PushNotificationChannel, PushNotificationReceivedEventArgs> handler =
                PushNotificationReceived;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        /// <summary>
        /// The register.
        /// </summary>
        public async void Register()
        {
            await TryRegisterChannel();

            var registration = new PushRegistration { InstallationId = Storage.GetInstallationId(), ChannelUri = channel.Uri };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/register", registration);
        }


        /// <summary>
        /// The un register.
        /// </summary>
        public async void UnRegister()
        {
            await TryRegisterChannel();

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>("api/push/unregister", registration);
        }

        /// <summary>
        /// The try register channel.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task<bool> TryRegisterChannel()
        {
            try
            {
                if (channel == null)
                {
                    channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    channel.PushNotificationReceived += OnPushNotificationReceived;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
