namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Tasks;

    /// <summary>
    /// The home.
    /// </summary>
    public partial class Home : PhoneApplicationPage
    {
        /// <summary>
        /// The subscription repository.
        /// </summary>
        private readonly ISubscriptionRepository subscriptionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home()
            : this(new SubscriptionRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="subscriptionRepository">
        /// The subscription Repository.
        /// </param>
        public Home(ISubscriptionRepository subscriptionRepository)
        {
            this.subscriptionRepository = subscriptionRepository;

            this.InitializeComponent();

            this.Recent = new SubscriptionViewModel { Name = "New Slaps" };
            this.Subscriptions = new ObservableCollection<SubscriptionViewModel> { this.Recent };

            var subscriptions = this.subscriptionRepository.GetSubsriptions();

            var mappedSubscriptions = new List<SubscriptionViewModel>();
            mappedSubscriptions = Mapper.Map(subscriptions, mappedSubscriptions);

            this.Subscriptions.AddRange(mappedSubscriptions);

            subscriptions.CollectionChanged += (sender, args) =>
                {
                    var newItems = new List<SubscriptionViewModel>();
                    newItems = Mapper.Map(args.NewItems, newItems);
                    this.Subscriptions.AddRange(newItems);

                    var oldItems = new List<SubscriptionViewModel>();
                    oldItems = Mapper.Map(args.NewItems, oldItems);
                    this.Subscriptions.RemoveRange(oldItems);
                };

            this.DataContext = this.Subscriptions;
        }

        /// <summary>
        /// Gets or sets the recent.
        /// </summary>
        public SubscriptionViewModel Recent { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; set; }

        /// <summary>
        /// Navigates to a view for creating or adding new streams.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event arguments.
        /// </param>
        private void NewStream_Click(object sender, EventArgs eventArgs)
        {
            this.Navigate("/Views/NewStream.xaml");
        }

        /// <summary>
        /// Refreshes the current stream.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event arguments.
        /// </param>
        private void Refresh_Click(object sender, EventArgs eventArgs)
        {
        }

        /// <summary>
        /// Navigates to a view for sharing a stream.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event arguments.
        /// </param>
        private void Share_Click(object sender, EventArgs eventArgs)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = "Code Samples";
            shareLinkTask.LinkUri = new Uri("http://code.msdn.com/wpapps", UriKind.Absolute);
            shareLinkTask.Message = "Here are some great code samples for Windows Phone.";

            shareLinkTask.Show();

            // this.Navigate("/Views/NewStream.xaml");
        }

        /// <summary>
        /// Navigates to a view for changing the settings.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event arguments.
        /// </param>
        private void Settings_Click(object sender, EventArgs eventArgs)
        {
            // this.Navigate("/Views/NewStream.xaml");
        }
    }
}