namespace Linkslap.WP.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

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
        /// The is running.
        /// </summary>
        private static volatile bool isRunning;

        /// <summary>
        /// The settings store.
        /// </summary>
        private readonly ISettingsStore settingsStore;

        /// <summary>
        /// The rest.
        /// </summary>
        private readonly Rest rest;

        private List<Link> links;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewSlapsStore"/> class.
        /// </summary>
        public NewSlapsStore()
            : this(new SettingsStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewSlapsStore"/> class.
        /// </summary>
        /// <param name="settingsStore">
        /// The settings Store.
        /// </param>
        public NewSlapsStore(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
            this.rest = new Rest();
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
                return this.links = this.GetLinks();
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
            if (!this.settingsStore.ShowInNewLinks(link.StreamKey))
            {
                if (NewSlapsChanged != null)
                {
                    NewSlapsChanged(this, link);
                }

                return;
            }

            var links = this.Links;

            // Link already exists but trigger a refresh
            if (links.Any(l => l.Id == link.Id))
            {
                if (NewSlapsChanged != null)
                {
                    NewSlapsChanged(this, link);
                }

                return;
            }

            this.links.Insert(0, link);
            Storage.Save(Key, this.links.Take(10).ToList());

            // this.UpdateBadge(links);

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
            Storage.Save(Key, links.Take(10).ToList());

            this.UpdateBadge(links);

            if (NewSlapsChanged != null)
            {
                NewSlapsChanged(this, link);
            }
        }

        public void Clear()
        {
            var links = new List<Link>();
            Storage.Save(Key, links.Take(10).ToList());

            this.UpdateBadge(links);

            if (NewSlapsChanged != null)
            {
                NewSlapsChanged(this, null);
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
            this.links = Storage.Load<List<Link>>(Key) ?? new List<Link>();
            if (this.links != null && (this.links.Any() || isRunning))
            {
                return this.links;
            }

            isRunning = true;
            this.links = new List<Link>();

            // return Storage.Load<List<Link>>(Key) ?? new List<Link>();

            this.rest.Get<List<Link>>(
                "api/link/user-latest",
                links =>
                    {
                        this.links.AddRange(links);
                        links.Reverse();
                        Storage.Save(Key, this.links.Take(10).ToList());

                        if (NewSlapsChanged != null)
                        {
                            foreach (var link in links)
                            {
                               NewSlapsChanged(this, link);
                            }
                        }
                        isRunning = false;
                    });

            return this.links;
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
