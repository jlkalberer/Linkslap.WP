namespace Linkslap.WP.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The subscription view model.
    /// </summary>
    public class SubscriptionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionViewModel"/> class.
        /// </summary>
        public SubscriptionViewModel()
        {
            this.Links = new ObservableCollection<LinkViewModel>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the stream key.
        /// </summary>
        public string StreamKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the links.
        /// </summary>
        public ObservableCollection<LinkViewModel> Links { get; private set; }

    }
}
