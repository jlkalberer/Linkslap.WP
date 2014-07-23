namespace Linkslap.WP.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The new slaps store.
    /// </summary>
    public class NewSlapsStore : INewSlapsStore
    {
        /// <summary>
        /// The key.
        /// </summary>
        private const string Key = "new-slaps";

        /// <summary>
        /// Initializes a new instance of the <see cref="NewSlapsStore"/> class.
        /// </summary>
        public NewSlapsStore()
        {
        }

        /// <summary>
        /// The new slaps changed.
        /// </summary>
        public static event EventHandler<Link> NewSlapsChanged;

        /// <summary>
        /// Gets the links.
        /// </summary>
        public IEnumerable<Link> Links
        {
            get
            {
                return this.GetLinks();
            }
        }

        /// <summary>
        /// The add link.
        /// </summary>
        /// <param name="link">
        /// The link.
        /// </param>
        public void AddLink(Link link)
        {
            var links = this.GetLinks();
            links.Add(link);
            Storage.Save(Key, links);

            this.UpdateBadge(links);

            if (NewSlapsChanged != null)
            {
                NewSlapsChanged(this, link);
            }
        }

        /// <summary>
        /// The remove link.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void RemoveLink(int id)
        {
            var links = this.GetLinks();
            var link = links.FirstOrDefault(l => l.Id == id);
            links.Remove(link);
            Storage.Save(Key, links);

            this.UpdateBadge(links);

            if (NewSlapsChanged != null)
            {
                NewSlapsChanged(this, link);
            }
        }

        /// <summary>
        /// The get links.
        /// </summary>
        /// <returns>
        /// The <see cref="List{Link}"/>.
        /// </returns>
        private List<Link> GetLinks()
        {
            return Storage.Load<List<Link>>(Key) ?? new List<Link>();
        }

        /// <summary>
        /// The update badge.
        /// </summary>
        /// <param name="links">
        /// The links.
        /// </param>
        private void UpdateBadge(IEnumerable<Link> links)
        {
            var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            var badgeElement = (XmlElement)badgeXml.SelectSingleNode("/badge");

            var count = links.Count();

            if (count == 0)
            {
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                return;
            }

            badgeElement.SetAttribute("value", count.ToString());
            var badge = new BadgeNotification(badgeXml);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badge);
        }
    }
}
