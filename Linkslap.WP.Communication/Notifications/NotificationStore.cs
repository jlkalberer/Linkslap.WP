using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linkslap.WP.Communication.Notifications
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using Windows.Networking.PushNotifications;

    using Linkslap.WP.Communication.Interfaces;

    using Microsoft.WindowsAzure.Messaging;

    using Subscription = Linkslap.WP.Communication.Models.Subscription;

    public class NotificationStore
    {
        /// <summary>
        /// The subscription store.
        /// </summary>
        private readonly ISubscriptionStore subscriptionStore;

        /// <summary>
        /// The hub.
        /// </summary>
        private readonly NotificationHub hub;

        /// <summary>
        /// The channel.
        /// </summary>
        private PushNotificationChannel channel;

        private ObservableCollection<Subscription> subscriptions;

        public NotificationStore(ISubscriptionStore subscriptionStore)
        {
            this.subscriptionStore = subscriptionStore;
            this.hub = new NotificationHub("<hub name>", "<connection string with listen access>");
        }

        /// <summary>
        /// The register.
        /// </summary>
        public void Register()
        {
            this.subscriptions = this.subscriptionStore.GetSubsriptions();
            this.subscriptions.CollectionChanged += (sender, args) => this.RegisterChannels();

            this.RegisterChannels();
        }

        /// <summary>
        /// The register channels.
        /// </summary>
        private async void RegisterChannels()
        {
            if (this.channel == null || this.channel.ExpirationTime < DateTime.Now)
            {
                this.channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            }

            await this.hub.RegisterNativeAsync(this.channel.Uri, this.subscriptions.Select(s => s.Stream.Key).ToArray());
        }
    }
}
