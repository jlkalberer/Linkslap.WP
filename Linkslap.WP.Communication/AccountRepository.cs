namespace Linkslap.WP.Communication
{
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using RestSharp;

    /// <summary>
    /// The account repository.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        public AccountRepository()
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
            var request = new RestRequest("token") { Method = Method.POST };
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", userName);
            request.AddParameter("password", password);

            var task = new TaskCompletionSource<Account>();
            this.rest.Execute<Account>(request, task.SetResult, task.SetCanceled);

            var account = await task.Task;
            Storage.Save("account", account);

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

            var request = new RestRequest("/api/account/userinfo");
            var task = new TaskCompletionSource<UserInfo>();
            this.rest.Execute<UserInfo>(request, task.SetResult, task.SetCanceled);
            //Task.WaitAll(task.Task);

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
