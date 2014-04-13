namespace Linkslap.WP.Communication.Interfaces
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Linkslap.WP.Communication.Models;

    /// <summary>
    /// The SubscriptionRepository interface.
    /// </summary>
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// The get subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{Feed}"/>.
        /// </returns>
        ObservableCollection<Subscription> GetSubsriptions();
    }
}