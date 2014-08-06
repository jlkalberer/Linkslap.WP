namespace Linkslap.WP.Communication.Interfaces
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Models;

    /// <summary>
    /// The SubscriptionStore interface.
    /// </summary>
    public interface ISubscriptionStore
    {
        /// <summary>
        /// The get subscriptions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{StreamKey}"/>.
        /// </returns>
        ObservableCollection<Subscription> GetSubsriptions();

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="streamKey">
        /// The stream id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Subscription> Add(string streamKey);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="subscriptionId">
        /// The subscription id.
        /// </param>
        void Delete(int subscriptionId);

        Subscription GetSubscription(int id);
    }
}