namespace Linkslap.WP.Communication.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// The link model.
    /// </summary>
    internal class LinkModel
    {
        /// <summary>
        /// Gets or sets the stream key.
        /// </summary>
        [JsonProperty("streamKey")]
        public string StreamKey { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the connection id.
        /// </summary>
        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
