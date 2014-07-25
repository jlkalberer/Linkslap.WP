﻿namespace Linkslap.WP.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The subscription repository.
    /// </summary>
    public class SubscriptionStore : ISubscriptionStore
    {
        /// <summary>
        /// The account repository.
        /// </summary>
        private readonly IAccountStore accountStore;

        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// The subscriptions.
        /// </summary>
        private ObservableCollection<Subscription> subscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionStore"/> class.
        /// </summary>
        public SubscriptionStore()
            : this(new AccountStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionStore"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account repository.
        /// </param>
        public SubscriptionStore(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
            this.rest = new Rest();
        }

        /// <summary>
        /// The new slaps changed.
        /// </summary>
        public static event EventHandler<Subscription> SubscriptionsChanged;

        /// <summary>
        /// The get subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{StreamKey}"/>.
        /// </returns>
        public ObservableCollection<Subscription> GetSubsriptions()
        {
            if (this.subscriptions != null)
            {
                return this.subscriptions;
            }

            this.subscriptions = Storage.Load<ObservableCollection<Subscription>>("subscriptions");

            if (this.subscriptions == null)
            {
                this.subscriptions = new ObservableCollection<Subscription>();
                Storage.Save("subscriptions", this.subscriptions);
            }

            this.rest.Get<List<Subscription>>(
                "api/subscription",
                values =>
                    {
                        var subs = this.subscriptions;
                        if (values == null || !values.Any())
                        {
                            subs.Clear();
                        }
                        else
                        {
                            subs.Remove(s => values.All(v => v.Id != s.Id));
                            subs.AddRange(values.Where(v => subs.All(s => s.Id != v.Id)));
                        }

                        
                        Storage.Save("subscriptions", subs);
                    });

            return this.subscriptions;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="streamId">
        /// The stream id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<Subscription> Add(string streamId)
        {
            var task = new TaskCompletionSource<Subscription>();

            this.rest.Post<Subscription>("api/subscription", new { id = streamId }, task.SetResult);

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
        public void Delete(int subscriptionId)
        {
            var uri = string.Format("api/subscription{0}", subscriptionId);
            this.rest.Delete<Subscription>(uri);

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
    }
}
