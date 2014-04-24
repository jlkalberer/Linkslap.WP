namespace Linkslap.WP.ViewModels
{
    /// <summary>
    /// The new stream view model.
    /// </summary>
    public class NewStreamViewModel : ViewModelBase
    {
        /// <summary>
        /// The busy.
        /// </summary>
        private bool busy;

        /// <summary>
        /// Gets or sets a value indicating whether busy.
        /// </summary>
        public bool Busy
        {
            get
            {
                return this.busy;
            }

            set
            {
                if (value.Equals(this.busy))
                {
                    return;
                }

                this.busy = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the stream name.
        /// </summary>
        public string StreamName { get; set; }
    }
}
