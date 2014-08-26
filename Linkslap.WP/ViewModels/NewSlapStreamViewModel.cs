namespace Linkslap.WP.ViewModels
{
    /// <summary>
    /// The new slap stream view model.
    /// </summary>
    public class NewSlapStreamViewModel : ViewModelBase
    {
        /// <summary>
        /// The stream name.
        /// </summary>
        private string streamName;

        private bool canSubmit;

        /// <summary>
        /// Gets or sets the stream name.
        /// </summary>
        public string StreamName
        {
            get
            {
                return this.streamName;
            }

            set
            {
                this.streamName = value;
                this.OnPropertyChanged();

                this.CanSubmit = !string.IsNullOrEmpty(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether can submit.
        /// </summary>
        public bool CanSubmit
        {
            get
            {
                return this.canSubmit;
            }
            set
            {
                this.canSubmit = value;
                this.OnPropertyChanged();
            }
        }
    }
}