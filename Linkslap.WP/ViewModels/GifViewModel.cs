namespace Linkslap.WP.ViewModels
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The gif view model.
    /// </summary>
    public class GifViewModel : ViewModelBase
    {
        private bool showGif;

        public string Thumbnail { get; set; }

        public string Gif { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show gif.
        /// </summary>
        public bool ShowGif
        {
            get
            {
                return this.showGif;
            }
            set
            {
                this.showGif = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("GifVisibility");
                this.OnPropertyChanged("ThumbnailVisibility");
            }
        }

        public Visibility GifVisibility
        {
            get
            {
                return this.ShowGif ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility ThumbnailVisibility
        {
            get
            {
                return !this.ShowGif ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
