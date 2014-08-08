namespace Linkslap.WP.BackgroundTask
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Notifications;

    using Windows.ApplicationModel.Background;

    /// <summary>
    /// The register tasks.
    /// </summary>
    public static class RegisterTasks
    {
        /// <summary>
        /// The run.
        /// </summary>
        public static void Run()
        {
            PushNotifications();
            RegisterPushNotifications();
        }

        /// <summary>
        /// The register task.
        /// </summary>
        private static async void PushNotifications()
        {
            const string PushNotificationTaskName = "ToastNotifications";

            if (GetRegisteredTask(PushNotificationTaskName) != null)
            {
                return;
            }

            var ns = new NotificationStore();
            ns.Register();

            await BackgroundExecutionManager.RequestAccessAsync();

            await ObtainLockScreenAccess();
            var taskBuilder = new BackgroundTaskBuilder
                                  {
                                      Name = PushNotificationTaskName,
                                      TaskEntryPoint = typeof(PushNotificationTask).FullName
                                  };

            var trigger = new PushNotificationTrigger();
            taskBuilder.SetTrigger(trigger);

            var internetCondition = new SystemCondition(SystemConditionType.InternetAvailable);
            taskBuilder.AddCondition(internetCondition);

            try
            {
                taskBuilder.Register();
            }
            catch (Exception exception)
            {
            }
        }

        /// <summary>
        /// The register push notifications.
        /// </summary>
        private static async void RegisterPushNotifications()
        {
            const string RegisterPushNotificationName = "RegisterNotifications";
            const int RegistrationInterval = 10 * 24 * 60;

            if (GetRegisteredTask(RegisterPushNotificationName) != null)
            {
                return;
            }

            await BackgroundExecutionManager.RequestAccessAsync();

            var taskBuilder = new BackgroundTaskBuilder
                                  {
                                      Name = RegisterPushNotificationName,
                                      TaskEntryPoint = typeof(RegisterPushNotificationTask).FullName
                                  };

            var trigger = new TimeTrigger(RegistrationInterval, false);
            taskBuilder.SetTrigger(trigger);

            var internetCondition = new SystemCondition(SystemConditionType.InternetAvailable);
            taskBuilder.AddCondition(internetCondition);

            try
            {
                taskBuilder.Register();
            }
            catch (Exception exception)
            {
            }
        }

        /// <summary>
        /// The obtain lock screen access.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task<bool> ObtainLockScreenAccess()
        {
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            if (status == BackgroundAccessStatus.Denied || status == BackgroundAccessStatus.Unspecified)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The get registered task.
        /// </summary>
        /// <param name="taskName">
        /// The task Name.
        /// </param>
        /// <returns>
        /// The <see cref="IBackgroundTaskRegistration"/>.
        /// </returns>
        private static IBackgroundTaskRegistration GetRegisteredTask(string taskName)
        {
            return BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(task => task.Name == taskName);
        }
    }
}
