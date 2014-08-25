namespace Linkslap.WP.Communication.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The gif me model.
    /// </summary>
    public class GifMeModel
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        [JsonProperty("meta")]
        public GifMeMeta Meta { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<GifMeResult> Results { get; set; }

        /// <summary>
        /// The gif me meta.
        /// </summary>
        public class GifMeMeta
        {
            /// <summary>
            /// Gets or sets the term.
            /// </summary>
            [JsonProperty("term")]
            public string Term { get; set; }

            /// <summary>
            /// Gets or sets the limit.
            /// </summary>
            [JsonProperty("limit")]
            public int Limit { get; set; }

            /// <summary>
            /// Gets or sets the page.
            /// </summary>
            [JsonProperty("page")]
            public int Page { get; set; }

            /// <summary>
            /// Gets or sets the total pages.
            /// </summary>
            [JsonProperty("total_pages")]
            public int TotalPages { get; set; }

            /// <summary>
            /// Gets or sets the total.
            /// </summary>
            [JsonProperty("total")]
            public int Total { get; set; }

            /// <summary>
            /// Gets or sets the timing.
            /// </summary>
            [JsonProperty("timing")]
            public string Timing { get; set; }
        }

        public class GifMeResult
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the score.
            /// </summary>
            [JsonProperty("score")]
            public string Score { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether nsfw.
            /// </summary>
            [JsonProperty("nswf")]
            public bool NSFW { get; set; }

            /// <summary>
            /// Gets or sets the link.
            /// </summary>
            [JsonProperty("link")]
            public string Link { get; set; }

            /// <summary>
            /// Gets or sets the thumb.
            /// </summary>
            [JsonProperty("thumb")]
            public string Thumb { get; set; }

            /// <summary>
            /// Gets or sets the created at.
            /// </summary>
            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
        }
    }
}
