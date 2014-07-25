namespace Linkslap.WP.Communication
{
    using System.Net;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Notifications;
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
            this.rest.Execute<Account>(message, "token", data, task.SetResult, task.SetException);

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
                return account;
            }

            var task = new TaskCompletionSource<UserInfo>();
            this.rest.Execute<UserInfo>(HttpMethod.Get, "/api/account/userinfo", null, task.SetResult, task.SetException);

            var userInfo = await task.Task;

            if (userInfo == null)
            {
                return account;
            }

            if (string.CompareOrdinal(account.UserName, userInfo.UserName) != 0)
            {
                account.UserName = userInfo.UserName;
                Storage.Save("account", account);
            }

            return account;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task Register(RegisterModel user)
        {
            var task = new TaskCompletionSource<object>();

            this.rest.Execute<dynamic>(
                HttpMethod.Post,
                "/api/account/register",
                user,
                result => task.SetResult(null),
                task.SetException);

            return task.Task;
        }

        /// <summary>
        /// The logout.
        /// </summary>
        public void Logout()
        {
            Storage.Save("account", null);

            var ns = new NotificationStore();
            ns.UnRegister();

            Storage.ClearAll();
        }
    }
}
