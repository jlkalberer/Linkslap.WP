namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.ViewModels;

    using Windows.ApplicationModel.DataTransfer.ShareTarget;
    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareLink
    {
        /// <summary>
        /// The subscription store.
        /// </summary>
        private readonly ISubscriptionStore subscriptionStore;

        /// <summary>
        /// The stream store.
        /// </summary>
        private readonly IStreamStore streamStore;

        /// <summary>
        /// The share operation.
        /// </summary>
        private ShareOperation shareOperation;

        /// <summary>
        /// The data.
        /// </summary>
        private Uri data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLink"/> class.
        /// </summary>
        public ShareLink()
            : this(new SubscriptionStore(), new StreamStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLink"/> class.
        /// </summary>
        /// <param name="subscriptionStore">
        /// The subscription store.
        /// </param>
        /// <param name="streamStore">
        /// The stream store.
        /// </param>
        public ShareLink(ISubscriptionStore subscriptionStore, IStreamStore streamStore)
        {
            this.subscriptionStore = subscriptionStore;
            this.streamStore = streamStore;
            this.InitializeComponent();

            this.Subscriptions = new ObservableCollection<SubscriptionViewModel>();
            this.DataContext = this.Subscriptions;
        }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; set; }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.shareOperation = e.Parameter as ShareOperation;

            MainPage.NotificationStore.Register();

            if (this.shareOperation == null)
            {
                return;
            }

            this.data = await this.shareOperation.Data.GetWebLinkAsync();

            var subscriptions = this.subscriptionStore.GetSubsriptions();

            var mappedSubscriptions = new List<SubscriptionViewModel>();
            mappedSubscriptions = Mapper.Map(subscriptions, mappedSubscriptions);

            this.Subscriptions.AddRange(mappedSubscriptions);

            subscriptions.CollectionChanged += (sender, args) => this.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                    {
                        var newItems = new List<SubscriptionViewModel>();
                        newItems = Mapper.Map(args.NewItems, newItems);
                        this.Subscriptions.AddRange(newItems);

                        var oldItems = new List<SubscriptionViewModel>();
                        oldItems = Mapper.Map(args.NewItems, oldItems);
                        this.Subscriptions.RemoveRange(oldItems);
                    });

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// The stream selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StreamSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var subscription = e.AddedItems[0] as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }

            var task = this.streamStore.SlapLink(subscription.StreamKey, "Test", this.data.ToString());
#if !DEBUG
            // This will shut down the app - we only want to do that if we are not debugging.
            task.ContinueWith(link => this.shareOperation.ReportCompleted());
#endif
        }
    }
}
