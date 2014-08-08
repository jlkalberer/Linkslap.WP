namespace Linkslap.WP.Communication
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The settings store.
    /// </summary>
    public class SettingsStore : ISettingsStore
    {
        /// <summary>
        /// The key.
        /// </summary>
        private const string Key = "SettingsStore";
        
        /// <summary>
        /// The subscription store.
        /// </summary>
        private readonly ISubscriptionStore subscriptionStore;

        /// <summary>
        /// The subscription settings.
        /// </summary>
        private ObservableCollection<SubscriptionSettings> subscriptionSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsStore"/> class.
        /// </summary>
        public SettingsStore()
            : this(new SubscriptionStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsStore"/> class.
        /// </summary>
        /// <param name="subscriptionStore">
        /// The subscription store.
        /// </param>
        public SettingsStore(ISubscriptionStore subscriptionStore)
        {
            this.subscriptionStore = subscriptionStore;
            var allSubscriptions = this.subscriptionStore.GetSubsriptions();
            var subscriptions = allSubscriptions.ToList();

            if (this.SubscriptionSettings != null)
            {
                subscriptions = subscriptions.Where(s => this.SubscriptionSettings.All(ss => ss.Id != s.Id)).ToList();
            }
            else
            {
                this.subscriptionSettings = new ObservableCollection<SubscriptionSettings>();
            }

            this.subscriptionSettings.AddRange(
                subscriptions.Select(s => new SubscriptionSettings
                    {
                        Id = s.Id,
                        ToastNotifications = true,
                        ShowNewLinks = true,
                        StreamKey = s.Stream.Key,
                        StreamName = s.Stream.Name
                    }));

            // Remove any stragglers
            this.subscriptionSettings.Remove(ss => allSubscriptions.All(s => s.Id != ss.Id));

            this.SaveSubscriptionSettings();
        }

        /// <summary>
        /// Gets or sets a value indicating whether disable all notifications.
        /// </summary>
        public bool DisableAllNotifications
        {
            get
            {
                return Storage.Load<bool>(Key + ".DisableAllNotifications");
            }

            set
            {
                Storage.Save(Key + ".DisableAllNotifications", value);
            }
        }

        /// <summary>
        /// Gets the subscription settings.
        /// </summary>
        public ObservableCollection<SubscriptionSettings> SubscriptionSettings
        {
            get
            {
                return this.subscriptionSettings
                       ?? (this.subscriptionSettings =
                           Storage.Load<ObservableCollection<SubscriptionSettings>>(Key + ".SubscriptionSettings"));
            }
        }

        /// <summary>
        /// The show push notification.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ShowPushNotification(string streamKey)
        {
            var subscriptionSetting = this.SubscriptionSettings.FirstOrDefault(s => s.StreamKey == streamKey);

            if (subscriptionSetting == null)
            {
                this.AddSubscription(streamKey);
                return !this.DisableAllNotifications;
            }

            return !this.DisableAllNotifications && subscriptionSetting.ToastNotifications;
        }

        /// <summary>
        /// The show in new links.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ShowInNewLinks(string streamKey)
        {
            var subscriptionSetting = this.SubscriptionSettings.FirstOrDefault(s => s.StreamKey == streamKey);

            if (subscriptionSetting == null)
            {
                this.AddSubscription(streamKey);
                return true;
            }

            return subscriptionSetting.ShowNewLinks;
        }

        /// <summary>
        /// The save subscription settings.
        /// </summary>
        public void SaveSubscriptionSettings()
        {
            Storage.Save(Key + ".SubscriptionSettings", this.subscriptionSettings);
        }

        /// <summary>
        /// The remove subscription setting.
        /// </summary>
        /// <param name="subscriptionId">
        /// The subscription Id.
        /// </param>
        public void RemoveSubscriptionSetting(int subscriptionId)
        {
            this.SubscriptionSettings.Remove(s => s.Id == subscriptionId);
        }

        /// <summary>
        /// The add subscription.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        private void AddSubscription(string streamKey)
        {
            var subscription = this.subscriptionStore.GetSubscription(streamKey);

            if (subscription == null)
            {
                return;
            }

            this.subscriptionSettings.Add(
                new SubscriptionSettings
                {
                    ToastNotifications = true,
                    ShowNewLinks = true,
                    StreamKey = streamKey,
                    StreamName = subscription.Stream.Name
                });

            this.SaveSubscriptionSettings();
        }
    }
}
