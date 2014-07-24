namespace Linkslap.WP.BackgroundTask
{
    using System;

    using Windows.ApplicationModel.Background;
    using Windows.Data.Xml.Dom;
    using Windows.Networking.PushNotifications;
    using Windows.UI.Notifications;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// The push notification task.
    /// </summary>
    public sealed class PushNotificationTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {           
            // Store the content received from the notification so it can be retrieved from the UI.
            var rawNotification = (RawNotification)taskInstance.TriggerDetails;


            if (rawNotification == null || string.IsNullOrEmpty(rawNotification.Content))
            {
                return;
            }

            var content = rawNotification.Content;
            dynamic jsonObject = JsonConvert.DeserializeObject(content);

            if (jsonObject.ObjectType == "submittedlink")
            {
                this.SendLinkNotification(content);
            }
            else if (jsonObject.ObjectType == "subscription")
            {
                this.AddSubscription(content);
            }
        }
        
        private bool SendLinkNotification(string content)
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
            store.AddLink(link);

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
            store.AddSubscription(subscription);

            return true;
        }
    }
}
