namespace Linkslap.WP.Communication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;

    using RestSharp;

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

        /// <summary>
        /// The get stream.
        /// </summary>
        /// <param name="streamKey">
        /// The stream key.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<List<Link>> GetStreamLinks(string streamKey)
        {
            var task = new TaskCompletionSource<List<Link>>();

            var request = new RestRequest("api/stream/{streamKey}/links");
            request.AddParameter("streamKey", streamKey, ParameterType.UrlSegment);
            
            var restClient = new Rest();
            restClient.Execute<List<Link>>(request, task.SetResult);

            return task.Task;
        }
    }
}
