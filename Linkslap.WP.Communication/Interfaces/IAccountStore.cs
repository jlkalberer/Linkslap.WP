namespace Linkslap.WP.Communication.Interfaces
{
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Models;

    /// <summary>
    /// The AccountStore interface.
    /// </summary>
    public interface IAccountStore
    {
        /// <summary>
        /// The authenticate.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="Account"/>.
        /// </returns>
        Task<Account> Authenticate(string userName, string password);

        /// <summary>
        /// Gets the currently authenticated account.
        /// </summary>
        /// <returns>
        /// The <see cref="Account"/>.
        /// </returns>
        Task<Account> Get();

        Task Register(RegisterModel user);
    }
}
