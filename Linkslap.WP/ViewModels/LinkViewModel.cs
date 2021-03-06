﻿namespace Linkslap.WP.ViewModels
{
    using System;

    /// <summary>
    /// The link view model.
    /// </summary>
    public class LinkViewModel : ViewModelBase
    {
        private Uri uri;

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
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the stream name.
        /// </summary>
        public string StreamName { get; set; }

        /// <summary>
        /// Gets or sets the stream key.
        /// </summary>
        public string StreamKey { get; set; }

        /// <summary>
        /// Gets the info.
        /// </summary>
        public string Info
        {
            get
            {
                return string.Format(
                    "Slapped at {0} by {1} to {2}",
                    this.CreatedDate.ToLocalTime().ToString("M/d/yyyy h:mm tt"),
                    this.UserName,
                    this.StreamName);
            }
        }

        /// <summary>
        /// Gets the uri.
        /// </summary>
        public Uri Uri
        {
            get
            {
                if (this.uri == null)
                {
                    this.uri = new Uri(this.Url, UriKind.RelativeOrAbsolute);
                }

                return this.uri;
            }

            set
            {
                this.uri = value;
                this.OnPropertyChanged();
            }
        }
    }
}
