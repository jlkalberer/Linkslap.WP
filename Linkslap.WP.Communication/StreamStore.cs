namespace Linkslap.WP.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;

    /// <summary>
    /// The stream repository.
    /// </summary>
    public class StreamStore : IStreamStore
    {
        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamStore"/> class.
        /// </summary>
        public StreamStore()
        {
            this.rest = new Rest();
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

            this.rest.Post<Stream>("api/stream", new { name = streamName }, task.SetResult);

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

            var uri = string.Format("api/stream/{0}/links", streamKey);
            
            this.rest.Get<List<Link>>(uri, task.SetResult);

            return task.Task;
        }

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
        public Task<Link> SlapLink(string streamKey, string comment, string url)
        {
            var model = new LinkModel
                            {
                                ConnectionId = string.Empty, // TODO - This needs to be set using the push notification ID..
                                Stream = streamKey, 
                                Comment = comment, 
                                Url = url
                            };

            var tcs = new TaskCompletionSource<Link>();
            this.rest.Post<Link>("api/link", model, tcs.SetResult);

            return tcs.Task;
        }
    }
}
