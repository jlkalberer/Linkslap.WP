namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.ObjectModel;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
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

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly ShareLinkViewModel viewModel;

        /// <summary>
        /// The share operation.
        /// </summary>
        private ShareOperation shareOperation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLink"/> class.
        /// </summary>
        public ShareLink()
            : this(new StreamStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareLink"/> class.
        /// </summary>
        /// <param name="streamStore">
        /// The stream store.
        /// </param>
        public ShareLink(IStreamStore streamStore)
        {
            this.streamStore = streamStore;
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

                this.viewModel.Uri = new Uri(model.Gif, UriKind.RelativeOrAbsolute);
                // this.viewModel.Comment = model.Gif;
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
                this.NavigationHelper.GoBack();
            }
        }
    }
}
