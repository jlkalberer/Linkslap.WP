namespace Linkslap.WP.ViewModels
{
    using System.Collections.ObjectModel;

    public class ViewStreamViewModel : ViewModelBase
    {
        public ViewStreamViewModel()
        {
            this.Links = new ObservableCollection<LinkViewModel>();
        }

        /// <summary>
        /// Gets or sets the stream name.
        /// </summary>
        public string StreamName { get; set; }

        /// <summary>
        /// Gets or sets the stream key.
        /// </summary>
        public string StreamKey { get; set; }

        public ObservableCollection<LinkViewModel> Links { get; private set; }

    }
}
