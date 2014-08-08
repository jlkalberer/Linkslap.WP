namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.ObjectModel;

    using Windows.UI.Xaml;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Windows.ApplicationModel.DataTransfer.ShareTarget;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareLink
    {
        /// <summary>
        /// The stream store.
        /// </summary>
        private readonly IStreamStore streamStore;

        private readonly SubscriptionStore subscriptionStore;

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly ShareLinkViewModel viewModel;

        /// <summary>
        /// The share operation.
        /// </summary>
        private ShareOperation shareOperation;

        private string streamKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLink"/> class.
        /// </summary>
        public ShareLink()
            : this(new StreamStore(), new SubscriptionStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLink"/> class.
        /// </summary>
        /// <param name="streamStore">
        ///     The stream store.
        /// </param>
        /// <param name="subscriptionStore"></param>
        public ShareLink(IStreamStore streamStore, SubscriptionStore subscriptionStore)
        {
            this.streamStore = streamStore;
            this.subscriptionStore = subscriptionStore;
            this.InitializeComponent();
            this.viewModel = this.DataContext as ShareLinkViewModel;
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
            if (e.Parameter is ShareOperation)
            {
                this.shareOperation = e.Parameter as ShareOperation;

                if (this.shareOperation == null)
                {
                    return;
                }

                this.viewModel.Uri = await this.shareOperation.Data.GetWebLinkAsync();
                this.viewModel.Comment = this.shareOperation.Data.Properties.Title;
            }
            else if (e.Parameter is GifViewModel)
            {
                var model = e.Parameter as GifViewModel;
                this.streamKey = model.StreamKey;
                this.viewModel.Uri = new Uri(model.Gif, UriKind.RelativeOrAbsolute);

                if (this.streamKey != null)
                {
                    this.StreamSelector.Visibility = Visibility.Collapsed;
                    this.ShareButton.Visibility = Visibility.Visible;
                }
            }

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

            var task = this.streamStore.SlapLink(subscription.StreamKey, this.viewModel.Comment, this.viewModel.Uri.ToString());

            if (this.shareOperation != null)
            {
                // This will shut down the app - we only want to do that if we are not debugging.
                task.ContinueWith(link => this.shareOperation.ReportCompleted());
            }
            else
            {
                this.NavigateRemoveFrames<ViewStream, FindGifs>(subscription);
            }
        }

        /// <summary>
        /// The share button clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ShareButtonClicked(object sender, RoutedEventArgs e)
        {
            this.streamStore.SlapLink(this.streamKey, this.viewModel.Comment, this.viewModel.Uri.ToString());

            var subscription = this.subscriptionStore.GetSubscription(this.streamKey);

            if (subscription == null)
            {
                this.Navigate<Home>();
            }
            else
            {
                var subscriptionViewModel = Mapper.Map<Subscription, SubscriptionViewModel>(subscription);
                this.Navigate<ViewStream>(subscriptionViewModel);    
            }
        }
    }
}
