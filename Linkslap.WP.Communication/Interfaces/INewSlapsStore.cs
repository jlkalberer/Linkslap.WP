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
    }
}
