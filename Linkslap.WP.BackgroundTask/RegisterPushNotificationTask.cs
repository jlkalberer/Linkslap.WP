namespace Linkslap.WP.BackgroundTask
{
    using Linkslap.WP.Communication.Notifications;

    using Windows.ApplicationModel.Background;

    /// <summary>
    /// The register push notification task.
    /// </summary>
    public sealed class RegisterPushNotificationTask : IBackgroundTask
    {
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="taskInstance">
        /// The task instance.
        /// </param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var store = new NotificationStore();
            store.Register();
        }
    }
}
