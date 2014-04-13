namespace Linkslap.WP.Communication.Interfaces
{
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Models;

    public interface IStreamRepository
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
    }
}