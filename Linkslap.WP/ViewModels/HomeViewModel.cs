namespace Linkslap.WP.ViewModels
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// The home view model.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        public HomeViewModel()
        {
            this.NewLinks = new ObservableCollection<LinkViewModel>();
            this.Subscriptions = new ObservableCollection<SubscriptionViewModel>();
        }

        /// <summary>
        /// Gets the new links.
        /// </summary>
        public ObservableCollection<LinkViewModel> NewLinks { get; private set; }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; private set; }
    }
}
