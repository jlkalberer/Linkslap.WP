namespace Linkslap.WP.Communication.Interfaces
{
    using System.Threading.Tasks;

    using Linkslap.WP.ViewModels;

    /// <summary>
    /// The GifStore interface.
    /// </summary>
    public interface IGifStore
    {
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
        Task<GifMeModel> Search(string query, bool nsfw, int page = 0);
    }
}
