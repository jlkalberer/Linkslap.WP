namespace Linkslap.WP.Views
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewStream : PageBase
    {
        /// <summary>
        /// The stream store.
        /// </summary>
        private readonly IStreamStore streamStore;

        private readonly ViewStreamViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStream"/> class.
        /// </summary>
        public ViewStream()
            : this(new StreamStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStream"/> class.
        /// </summary>
        /// <param name="streamStore">
        /// The stream store.
        /// </param>
        public ViewStream(IStreamStore streamStore)
        {
            this.streamStore = streamStore;

            this.InitializeComponent();

            this.viewModel = this.DataContext as ViewStreamViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="eventArgs">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            var subscription = eventArgs.Parameter as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }

            this.viewModel.StreamName = subscription.Name;
            this.viewModel.Links.AddRange(subscription.Links);

            var task = this.streamStore.GetStreamLinks(subscription.StreamKey);

            task.ContinueWith(
                links => this.Run(
                    () =>
                        {
                            var result = Mapper.Map(links.Result, new List<LinkViewModel>());
                            this.viewModel.Links.AddRange(result.OrderByDescending(l => l.CreatedDate));
                        }));

            base.OnNavigatedTo(eventArgs);
        }

        /// <summary>
        /// The link long list_ on selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LinkLongList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Navigate<View>(new ViewLinksViewModel(e.AddedItems[0] as LinkViewModel, this.viewModel.Links));
        }
    }
}
