namespace Linkslap.WP.Communication.Models
{
    /// <summary>
    /// The subscription.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the feed.
        /// </summary>
        public virtual Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether administer.
        /// </summary>
        public bool Administer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether read.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether write.
        /// </summary>
        public bool Write { get; set; }
    }
}
