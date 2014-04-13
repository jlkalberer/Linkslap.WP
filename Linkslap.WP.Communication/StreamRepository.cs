namespace Linkslap.WP.Communication
{
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;

    /// <summary>
    /// The stream repository.
    /// </summary>
    public class StreamRepository : IStreamRepository
    {
        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepository"/> class.
        /// </summary>
        public StreamRepository()
        {
            this.rest = new Rest("api/stream");
        }

        /// <summary>
        /// The new stream.
        /// </summary>
        /// <param name="streamName">
        /// The stream name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<Stream> NewStream(string streamName)
        {
            var task = new TaskCompletionSource<Stream>();

            this.rest.Post<Stream>(new { name = streamName }, task.SetResult);

            return task.Task;
        }
    }
}
