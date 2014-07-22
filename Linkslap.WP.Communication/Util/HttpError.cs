namespace Linkslap.WP.Communication.Util
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The http error.
    /// </summary>
    public class HttpError : Exception
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty("Message")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the model state.
        /// </summary>
        public Dictionary<string, List<string>> ModelState { get; set; }
    }
}
