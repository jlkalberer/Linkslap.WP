namespace Linkslap.WP.Communication
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The subscription repository.
    /// </summary>
    public class SubscriptionRepository : ISubscriptionRepository
    {
        /// <summary>
        /// The account repository.
        /// </summary>
        private readonly IAccountRepository accountRepository;

        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// The subscriptions.
        /// </summary>
        private ObservableCollection<Subscription> subscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionRepository"/> class.
        /// </summary>
        public SubscriptionRepository()
            : this(new AccountRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionRepository"/> class.
        /// </summary>
        /// <param name="accountRepository">
        /// The account repository.
        /// </param>
        public SubscriptionRepository(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
            this.rest = new Rest("api/subscription");
        }

        /// <summary>
        /// The get subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{Feed}"/>.
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

            this.rest.Get<List<Subscription>>(values =>
            {
                if (values == null || !values.Any())
                {
                    return;
                }

                var subs = this.subscriptions;

                if (subs == null)
                {
                    return;
                }

                subs.Remove(s => values.All(v => v.Id != s.Id));
                subs.AddRange(values.Where(v => subs.Any(s => s.Id == v.Id)));
            });

            return this.subscriptions;
        }
    }
}
