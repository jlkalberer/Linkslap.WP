using System.Threading.Tasks;

namespace Linkslap.WP.Communication
{
    using Windows.Web.Http;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.ViewModels;

    public class GifStore : IGifStore
    {
        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// Initializes a new instance of the <see cref="GifStore"/> class.
        /// </summary>
        public GifStore()
        {
            this.rest = new Rest("http://api.gifme.io");
        }

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="nsfw">
        /// The results can be NSFW.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<GifMeModel> Search(string query, bool nsfw, int page = 0)
        {
            var task = new TaskCompletionSource<GifMeModel>();

            this.rest.Execute<GifMeModel>(
                HttpMethod.Get,
                "/v1/search",
                new { key = "MNfCaCC9tRAr3yzf", query, sfw = nsfw, limit = 20, page },
                task.SetResult,
                task.SetException);

            return task.Task;
        }
    }
}
