namespace Linkslap.WP.Communication.Interfaces
{
    using System.Collections.Generic;

    using Linkslap.WP.Communication.Models;

    public interface INewSlapsStore
    {
        /// <summary>
        /// Gets the links.
        /// </summary>
        IEnumerable<Link> Links { get; }

        /// <summary>
        /// The add link.
        /// </summary>
        /// <param name="link">
        /// The link.
        /// </param>
        void AddLink(Link link);

        /// <summary>
        /// The remove link.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        void RemoveLink(int id);

        /// <summary>
        /// The clear.
        /// </summary>
        void Clear();
    }
}
