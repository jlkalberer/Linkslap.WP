namespace Linkslap.WP.Communication.Models
{
    /// <summary>
    /// The link model.
    /// </summary>
    internal class LinkModel
    {
        /// <summary>
        /// Gets or sets the feed.
        /// </summary>
        public string Stream { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the connection id.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }
    }
}
