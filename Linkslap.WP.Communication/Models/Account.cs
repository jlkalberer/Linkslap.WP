namespace Linkslap.WP.Communication.Models
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// The account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the bearer token.
        /// </summary>
        [JsonProperty("access_token")]
        public string BearerToken { get; set; }

        /// <summary>
        /// Gets or sets the token issued.
        /// </summary>
        [JsonProperty(".issued")]
        public DateTime TokenIssued { get; set; }

        /// <summary>
        /// Gets or sets the token expires.
        /// </summary>
        [JsonProperty(".expires")]
        public DateTime TokenExpires { get; set; }
    }
}
