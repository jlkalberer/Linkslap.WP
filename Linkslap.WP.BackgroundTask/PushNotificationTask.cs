namespace Linkslap.WP.BackgroundTask
{
    using System.Diagnostics;

    using Windows.ApplicationModel.Background;
    using Windows.Networking.PushNotifications;
    using Windows.Storage;
    using Windows.UI.Notifications;

    /// <summary>
    /// The push notification task.
    /// </summary>
    public sealed class PushNotificationTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {           
            // Get the background task details
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            string taskName = taskInstance.Task.Name;

            Debug.WriteLine("Background " + taskName + " starting...");

            // Store the content received from the notification so it can be retrieved from the UI.
            RawNotification rawNotification = (RawNotification)taskInstance.TriggerDetails;
            settings.Values[taskName] = rawNotification.Content;

            Debug.WriteLine("Background " + taskName + " completed!");

            var notification = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            var toast = new ToastNotification(notification) { };

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
