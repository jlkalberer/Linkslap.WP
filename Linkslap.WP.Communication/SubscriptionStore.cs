namespace Linkslap.WP.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The subscription repository.
    /// </summary>
    public class SubscriptionStore : ISubscriptionStore
    {
        /// <summary>
        /// The subscriptions.
        /// </summary>
        private static ObservableCollection<Subscription> subscriptions;

        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionStore"/> class.
        /// </summary>
        public SubscriptionStore()
        {
            this.rest = new Rest();
        }

        /// <summary>
        /// The new slaps changed.
        /// </summary>
        public static event EventHandler<Subscription> SubscriptionsChanged;

        /// <summary>
        /// The no subscriptions detected.
        /// </summary>
        public static event EventHandler NoSubscriptionsDetected;

        /// <summary>
        /// The get subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{StreamKey}"/>.
        /// </returns>
        public ObservableCollection<Subscription> GetSubsriptions()
        {
            // If the channel is null then push notifications are probably turned off...
            if (subscriptions != null)
            {
                return subscriptions;
            }

            subscriptions = Storage.Load<ObservableCollection<Subscription>>("subscriptions");

            if (subscriptions == null)
            {
                subscriptions = new ObservableCollection<Subscription>();
                Storage.Save("subscriptions", subscriptions);
            }

            this.rest.Get<List<Subscription>>(
                "api/subscription",
                values =>
                    {
                        var subs = subscriptions;
                        if (values == null || !values.Any())
                        {
                            subs.Clear();

                            if (NoSubscriptionsDetected != null)
                            {
                                NoSubscriptionsDetected(null, null);
                            }
                        }
                        else
                        {
                            subs.Remove(s => values.All(v => v.Id != s.Id));
                            subs.AddRange(values.Where(v => subs.All(s => s.Id != v.Id)));
                        }

                        Storage.Save("subscriptions", subs);
                    });

            return subscriptions;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="streamKey">
        /// The stream id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<Subscription> Add(string streamKey)
        {
            var task = new TaskCompletionSource<Subscription>();

            var subs = this.GetSubsriptions();
            var subscription = subs.FirstOrDefault(s => s.Stream.Key == streamKey);
            if (subscription != null)
            {
                task.SetResult(subscription);
                return task.Task;
            }

            this.rest.Post<Subscription>("api/subscription/join", new { streamKey }, task.SetResult);

            var ns = new NotificationStore();
            ns.Register();

            return task.Task;
        }

        public void AddSubscription(Subscription subscription)
        {
            var subscriptions = this.GetSubsriptions();

            if (subscriptions.Any(s => s.Id == subscription.Id))
            {
                if (SubscriptionsChanged != null)
                {
                    SubscriptionsChanged(this, subscription);
                }

                return;
            }

            subscriptions.Add(subscription);
            Storage.Save("subscriptions", subscriptions);

            var ns = new NotificationStore();
            ns.Register();

            if (SubscriptionsChanged != null)
            {
                SubscriptionsChanged(this, subscription);
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="subscriptionId">
        /// The subscription id.
        /// </param>
        public void Delete(string streamKey)
        {
            this.Remove(streamKey);

            var uri = string.Format("api/subscription?streamKey={0}", streamKey);
            this.rest.Delete<Subscription>(uri);

            var ns = new NotificationStore();
            ns.Register();

            if (SubscriptionsChanged != null)
            {
                SubscriptionsChanged(this, null);
            }
        }

        /// <summary>
        /// The get subscription.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Subscription"/>.
        /// </returns>
        public Subscription GetSubscription(int id)
        {
            return Storage.Load<ObservableCollection<Subscription>>("subscriptions").FirstOrDefault(s => s.Id == id);
        }

        public Subscription GetSubscription(string streamKey)
        {
            return this.GetSubsriptions().FirstOrDefault(s => s.Stream.Key == streamKey);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        public void Remove(string streamKey)
        {
            var subs = this.GetSubsriptions();
            subs.Remove(s => s.Stream.Key == streamKey);
            Storage.Save("subscriptions", subs);
        }
    }
}
