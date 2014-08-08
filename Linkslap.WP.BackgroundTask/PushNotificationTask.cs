namespace Linkslap.WP.BackgroundTask
{
    using System;
    using System.Linq;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Models;

    using Newtonsoft.Json;

    using Windows.ApplicationModel.Background;
    using Windows.Data.Xml.Dom;
    using Windows.Networking.PushNotifications;
    using Windows.UI.Notifications;

    /// <summary>
    /// The push notification task.
    /// </summary>
    public sealed class PushNotificationTask : IBackgroundTask
    {
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="taskInstance">
        /// The task instance.
        /// </param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {           
            // Store the content received from the notification so it can be retrieved from the UI.
            var rawNotification = (RawNotification)taskInstance.TriggerDetails;


            if (rawNotification == null || string.IsNullOrEmpty(rawNotification.Content))
            {
                return;
            }

            this.Process(rawNotification.Content, true);
        }

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="showNotifications">
        /// The show notifications.
        /// </param>
        public void Process(string content, bool showNotifications)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(content);
            
            if (jsonObject.ObjectType == "submittedlink")
            {
                SendLinkNotification(content, showNotifications);
            }
            else if (jsonObject.ObjectType == "subscription")
            {
                this.AddSubscription(content);
            }
            else if (jsonObject.streamKey != null)
            {
                var store = new SubscriptionStore();
                store.Remove((string)jsonObject.streamKey);
            }
        }

        /// <summary>
        /// The send link notification.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="showNotifications">
        /// The show notifications.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool SendLinkNotification(string content, bool showNotifications)
        {
            Link link;
            try
            {
                link = JsonConvert.DeserializeObject<Link>(content);
            }
            catch (Exception exception)
            {
                return false;
            }

            var store = new NewSlapsStore();

            if (store.Links.All(l => l.Id != link.Id))
            {
                store.AddLink(link);
            }
            

            // Do not show notification if processing from inside the app.
            if (!showNotifications)
            {
                return true;
            }


            var settings = new SettingsStore();
            if (!settings.ShowPushNotification(link.StreamKey))
            {
                return true;
            }

            var notification = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            var toastElement = ((XmlElement)notification.SelectSingleNode("/toast"));
            toastElement.SetAttribute("launch", link.Id.ToString());

            var badgeElements = notification.DocumentElement.SelectNodes(".//text");

            badgeElements[0].InnerText = "Linkslap";
            badgeElements[1].InnerText = "New link in " + link.StreamName;
            
            dynamic toast = new ToastNotification(notification); //{ Tag = link.Id.ToString() };
            toast.Tag = link.Id.ToString();

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            return true;
        }

        /// <summary>
        /// The add subscription.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool AddSubscription(string content)
        {
            Subscription subscription;
            try
            {
                subscription = JsonConvert.DeserializeObject<Subscription>(content);
            }
            catch (Exception exception)
            {
                return false;
            }

            var store = new SubscriptionStore();

            if (store.GetSubsriptions().Any(s => s.Id == subscription.Id))
            {
                return false;
            }

            store.AddSubscription(subscription);

            return true;
        }
    }
}
