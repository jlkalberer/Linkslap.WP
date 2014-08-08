namespace Linkslap.WP.Communication.Models
{
    public class SubscriptionSettings
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the stream key.
        /// </summary>
        public string StreamKey { get; set; }

        /// <summary>
        /// Gets or sets the stream name.
        /// </summary>
        public string StreamName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether push notifications.
        /// </summary>
        public bool ToastNotifications { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show new links.
        /// </summary>
        public bool ShowNewLinks { get; set; }
    }
}
