namespace Linkslap.WP.Communication
{
    using System.Net;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Windows.Storage.Streams;
    using Windows.Web.Http;

    /// <summary>
    /// The account repository.
    /// </summary>
    public class AccountStore : IAccountStore
    {
        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountStore"/> class.
        /// </summary>
        public AccountStore()
        {
            this.rest = new Rest();
        }

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
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Account> Authenticate(string userName, string password)
        {
            var message = new HttpRequestMessage();
            userName = WebUtility.UrlEncode(userName);
            password = WebUtility.UrlEncode(password);
            var data =
                new HttpStringContent(
                    string.Format("grant_type=password&username={0}&password={1}", userName, password),
                    UnicodeEncoding.Utf8,
                    "application/x-www-form-urlencoded");

            var task = new TaskCompletionSource<Account>();
            this.rest.Execute<Account>(message, "token", data, task.SetResult, task.SetCanceled);

            var account = await task.Task;

            if (task.Task.IsCompleted)
            {
                Storage.Save("account", account);
            }

            return account;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Account> Get()
        {
            var account = Storage.Load<Account>("account");

            if (account == null || string.IsNullOrEmpty(account.BearerToken))
            {
                return null;
            }

            var task = new TaskCompletionSource<UserInfo>();
            this.rest.Execute<UserInfo>(HttpMethod.Get, "/api/account/userinfo", null, task.SetResult, task.SetCanceled);

            var userInfo = await task.Task;


            if (userInfo == null)
            {
                return null;
            }

            if (string.CompareOrdinal(account.UserName, userInfo.UserName) != 0)
            {
                account.UserName = userInfo.UserName;
                Storage.Save("account", account);
            }

            return account;
        }
    }
}
