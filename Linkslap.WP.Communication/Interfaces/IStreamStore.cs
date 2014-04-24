namespace Linkslap.WP.Communication.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Models;

    public interface IStreamStore
    {
        /// <summary>
        /// The new stream.
        /// </summary>
        /// <param name="streamName">
        /// The stream name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Stream> NewStream(string streamName);

        /// <summary>
        /// The get stream links.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<Link>> GetStreamLinks(string streamKey);

        /// <summary>
        /// The slap link.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Link> SlapLink(string streamKey, string comment, string url);
    }
}