namespace Linkslap.WP.Communication.Notifications
{
    using System;

    using Windows.Data.Xml.Dom;

    using AutoMapper;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Newtonsoft.Json;

    using Windows.Networking.PushNotifications;
    using Windows.UI.Notifications;

    /// <summary>
    /// The notification store.
    /// </summary>
    public class NotificationStore
    {
        /// <summary>
        /// The channel.
        /// </summary>
        private PushNotificationChannel channel;

        /// <summary>
        /// The register.
        /// </summary>
        public async void Register()
        {
            if (this.channel == null || this.channel.ExpirationTime < DateTime.Now)
            {
                this.channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                // this.channel.PushNotificationReceived += this.ChannelOnPushNotificationReceived;
            }

            var registration = new PushRegistration
                                   {
                                       InstallationId = Storage.GetInstallationId(),
                                       ChannelUri = this.channel.Uri
                                   };

            var rest = new Rest();
            rest.Post<dynamic>(
                "api/register",
                registration,
                value =>
                {
                    var v = 0;
                });
        }

        /// <summary>
        /// The channel on push notification received.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void ChannelOnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType != PushNotificationType.Raw)
            {
                return;
            }

            if (args.RawNotification == null || string.IsNullOrEmpty(args.RawNotification.Content))
            {
                return;
            }

            var submittedLink = args.RawNotification.Content;

            var link = JsonConvert.DeserializeObject<Link>(submittedLink);

            var store = new NewSlapsStore();
            store.AddLink(link);

            var notification = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            var node = notification.CreateElement("Tag");
            node.InnerText = link.Id.ToString();
            notification.AppendChild(node);

            var toast = new ToastNotification(notification); // { Tag = link.Id.ToString() };
            
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
