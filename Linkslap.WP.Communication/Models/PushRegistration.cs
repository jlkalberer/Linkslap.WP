namespace Linkslap.WP.Communication.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// The push registration.
    /// </summary>
    internal class PushRegistration
    {
        /// <summary>
        /// Gets the platform.
        /// </summary>
        [JsonProperty("platform")]
        public string Platform
        {
            get
            {
                return "windows";
            }
        }

        /// <summary>
        /// Gets or sets the installation id.
        /// </summary>
        [JsonProperty("installationId")]
        public string InstallationId { get; set; }

        /// <summary>
        /// Gets or sets the channel uri.
        /// </summary>
        [JsonProperty("channelUri")]
        public string ChannelUri { get; set; }

        /// <summary>
        /// Gets or sets the device token.
        /// </summary>
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }
    }
}
