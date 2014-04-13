namespace Linkslap.WP.Communication.Models
{
    /// <summary>
    /// The user info.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has registered.
        /// </summary>
        public bool HasRegistered { get; set; }

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public string LoginProvider { get; set; }
    }
}
