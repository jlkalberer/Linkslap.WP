namespace Linkslap.WP.ViewModels
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// The view link view model.
    /// </summary>
    public class ViewLinksViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLinksViewModel"/> class.
        /// </summary>
        public ViewLinksViewModel()
            : this(null, new ObservableCollection<LinkViewModel>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLinksViewModel"/> class.
        /// </summary>
        /// <param name="linkViewModel">
        /// The link view model.
        /// </param>
        public ViewLinksViewModel(LinkViewModel linkViewModel)
            : this(linkViewModel, new ObservableCollection<LinkViewModel> { linkViewModel })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLinksViewModel"/> class.
        /// </summary>
        /// <param name="linkViewModel">
        /// The link view model.
        /// </param>
        /// <param name="links">
        /// The links.
        /// </param>
        public ViewLinksViewModel(LinkViewModel linkViewModel, ObservableCollection<LinkViewModel> links)
        {
            this.Links = links;
            this.SelectedItem = linkViewModel;
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public LinkViewModel SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public ObservableCollection<LinkViewModel> Links { get; set; }
    }
}
