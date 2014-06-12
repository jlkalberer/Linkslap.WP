namespace Linkslap.WP.ViewModels
{
    using System;

    /// <summary>
    /// The link view model.
    /// </summary>
    public class LinkViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        // public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        // public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
