namespace Linkslap.WP.Communication
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The subscription repository.
    /// </summary>
    public class SubscriptionRepository
    {
        /// <summary>
        /// The account repository.
        /// </summary>
        private IAccountRepository accountRepository;

        /// <summary>
        /// The subscriptions.
        /// </summary>
        private IEnumerable<Subscription> subscriptions;

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
        }

        /// <summary>
        /// The get subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{Feed}"/>.
        /// </returns>
        public IEnumerable<Subscription> GetFeeds()
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

            return this.subscriptions;
        }
    }
}
