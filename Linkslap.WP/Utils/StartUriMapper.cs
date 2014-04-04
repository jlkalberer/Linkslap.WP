namespace Linkslap.WP.Utils
{
    using System;
    using System.Windows.Navigation;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;

    /// <summary>
    /// The start uri mapper.
    /// </summary>
    public class StartUriMapper : UriMapperBase
    {
        /// <summary>
        /// The account repository.
        /// </summary>
        private readonly IAccountRepository accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUriMapper"/> class.
        /// </summary>
        public StartUriMapper()
            : this(new AccountRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUriMapper"/> class.
        /// </summary>
        /// <param name="accountRepository">
        /// The account repository.
        /// </param>
        public StartUriMapper(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// The map uri.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public override Uri MapUri(Uri uri)
        {
            var account = this.accountRepository.Get();

            if (account == null)
            {
                uri = new Uri("/Views/Login.xaml", UriKind.Relative);
            }
            else
            {
                uri = new Uri("/Views/Main.xaml", UriKind.Relative);
            }

            return uri;
        }
    }
}
