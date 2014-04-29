namespace Linkslap.WP.Communication.Util
{
    using System.Diagnostics;

    using Windows.ApplicationModel.Background;
    using Windows.Networking.PushNotifications;
    using Windows.Storage;

    public class PushNotificationTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get the background task details
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            string taskName = taskInstance.Task.Name;

            Debug.WriteLine("Background " + taskName + " starting...");

            // Store the content received from the notification so it can be retrieved from the UI.
            RawNotification notification = (RawNotification)taskInstance.TriggerDetails;
            settings.Values[taskName] = notification.Content;

            Debug.WriteLine("Background " + taskName + " completed!");
        }
    }
}
