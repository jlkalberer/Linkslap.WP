namespace Linkslap.WP.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The share link view model.
    /// </summary>
    public class ShareLinkViewModel : ViewModelBase
    {
        /// <summary>
        /// The stream store.
        /// </summary>
        private readonly IStreamStore streamStore;

        private string comment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLinkViewModel"/> class.
        /// </summary>
        public ShareLinkViewModel()
            : this(new SubscriptionStore(), new StreamStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLinkViewModel"/> class.
        /// </summary>
        /// <param name="subscriptionStore">
        /// The subscription store.
        /// </param>
        /// <param name="streamStore">
        /// The stream Store.
        /// </param>
        public ShareLinkViewModel(ISubscriptionStore subscriptionStore, IStreamStore streamStore)
        {
            this.streamStore = streamStore;
            this.Subscriptions = new ObservableCollection<SubscriptionViewModel>();
            var subscriptions = subscriptionStore.GetSubsriptions();

            var mappedSubscriptions = new List<SubscriptionViewModel>();
            mappedSubscriptions = Mapper.Map(subscriptions, mappedSubscriptions);

            this.Subscriptions.AddRange(mappedSubscriptions);

            subscriptions.CollectionChanged += (sender, args)  =>
                {
                    var newItems = new List<SubscriptionViewModel>();
                    newItems = Mapper.Map(args.NewItems, newItems);
                    this.Subscriptions.AddRange(newItems);

                    var oldItems = new List<SubscriptionViewModel>();
                    oldItems = Mapper.Map(args.NewItems, oldItems);
                    this.Subscriptions.RemoveRange(oldItems);
                };
        }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;

                this.OnPropertyChanged("CanSubmit");
            }
        }

        /// <summary>
        /// Gets a value indicating whether can submit.
        /// </summary>
        public bool CanSubmit
        {
            get
            {
                return !string.IsNullOrEmpty(this.Comment);
            }
        }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; private set; }
    }
}
