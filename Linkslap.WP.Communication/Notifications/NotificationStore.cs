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
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Microsoft.WindowsAzure.Messaging;

    using Subscription = Linkslap.WP.Communication.Models.Subscription;

    public class NotificationStore
    {
        /// <summary>
        /// The subscription store.
        /// </summary>
        private readonly ISubscriptionStore subscriptionStore;

        /// <summary>
        /// The channel.
        /// </summary>
        private PushNotificationChannel channel;

        /// <summary>
        /// The subscriptions.
        /// </summary>
        private ObservableCollection<Subscription> subscriptions;

        private NotificationHub hub;

        public NotificationStore()
            : this(new SubscriptionStore())
        {
            
        }

        public NotificationStore(ISubscriptionStore subscriptionStore)
        {
            this.subscriptionStore = subscriptionStore;
        }

        /// <summary>
        /// The register.
        /// </summary>
        public async void Register()
        {
            if (this.channel == null || this.channel.ExpirationTime < DateTime.Now)
            {
                this.channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                this.channel.PushNotificationReceived += (sender, args) =>
                    {
                        var v = 0;
                    };
            }

            //this.subscriptions = this.subscriptionStore.GetSubsriptions();
            //this.subscriptions.CollectionChanged += (sender, args) => this.RegisterChannels();

            var registration = new PushRegistration();
            registration.InstallationId = Storage.GetInstallationId();
            registration.ChannelUri = this.channel.Uri;

            var rest = new Rest();
            rest.Post<dynamic>(
                "api/register",
                registration,
                value =>
                {
                    var v = 0;
                });

            this.RegisterChannels();
        }

        /// <summary>
        /// The register channels.
        /// </summary>
        private async void RegisterChannels()
        {
            //this.hub = new NotificationHub(AppSettings.NotificationHubPath, AppSettings.HubConnectionString);

           //var registration = await this.hub.RegisterNativeAsync(this.channel.Uri);
            var v = 0;
        }
    }
}
