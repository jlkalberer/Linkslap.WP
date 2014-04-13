namespace Linkslap.WP.ViewModels
{
    using System.Collections.ObjectModel;

    using Linkslap.WP.Communication.Models;

    /// <summary>
    /// The home view model.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<LinkViewModel> newLinks;
 
        public HomeViewModel()
        {
            this.NewLinks = new ObservableCollection<LinkViewModel>();
            this.Subscriptions = new ObservableCollection<SubscriptionViewModel>();
        }

        /// <summary>
        /// Gets the new links.
        /// </summary>
        public ObservableCollection<LinkViewModel> NewLinks 
        {
            get
            {
                return this.newLinks;
            }

            private set
            {
                if (value.Equals(this.newLinks))
                {
                    return;
                }

                this.newLinks = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; private set; }
    }
}
