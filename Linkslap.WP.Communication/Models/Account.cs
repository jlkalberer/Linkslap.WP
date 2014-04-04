namespace Linkslap.WP.Communication.Models
{
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
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the bearer token.
        /// </summary>
        public string BearerToken { get; set; }
    }
}
