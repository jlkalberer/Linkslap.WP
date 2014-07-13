namespace Linkslap.WP.Communication.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// The feed.
    /// </summary>
    public class Stream
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the feed name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
