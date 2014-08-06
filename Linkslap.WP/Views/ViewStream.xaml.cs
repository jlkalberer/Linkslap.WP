namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.DataTransfer;
    using Windows.UI.Xaml;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
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

        private readonly AccountStore accountStore;

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly ViewStreamViewModel viewModel;

        private DataTransferManager dataTransferManager;

        private Task<Account> account;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStream"/> class.
        /// </summary>
        public ViewStream()
            : this(new StreamStore(), new AccountStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStream"/> class.
        /// </summary>
        /// <param name="streamStore">
        ///     The stream store.
        /// </param>
        /// <param name="accountStore"></param>
        public ViewStream(IStreamStore streamStore, AccountStore accountStore)
        {
            this.streamStore = streamStore;
            this.accountStore = accountStore;

            this.InitializeComponent();

            this.viewModel = this.DataContext as ViewStreamViewModel;
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += this.ShareStream;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="eventArgs">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            this.account = this.accountStore.Get();
            var subscription = eventArgs.Parameter as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }

            this.viewModel.StreamName = subscription.Name;
            this.viewModel.StreamKey = subscription.StreamKey;
            this.viewModel.Links.AddRange(subscription.Links);

            var task = this.streamStore.GetStreamLinks(subscription.StreamKey);

            task.ContinueWith(
                links => this.Run(
                    () =>
                        {
                            var result = Mapper.Map(links.Result, new List<LinkViewModel>());
                            this.viewModel.Links.AddRange(result.OrderByDescending(l => l.CreatedDate));
                        }));

            NewSlapsStore.NewSlapsChanged += (sender, link) => this.CrossThread(
                () =>
                    {
                        if (link == null || link.StreamKey != this.viewModel.StreamKey)
                        {
                            return;
                        }

                        this.viewModel.Links.Insert(0, Mapper.Map<Link, LinkViewModel>(link));
                    });

            base.OnNavigatedTo(eventArgs);
        }

        /// <summary>
        /// The on navigated from.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.dataTransferManager.DataRequested -= this.ShareStream;
            base.OnNavigatedFrom(e);
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

        /// <summary>
        /// The reply click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReplyClick(object sender, RoutedEventArgs e)
        {
            this.NavigateReplace<FindGifs>(this.viewModel.StreamKey);
        }

        /// <summary>
        /// The share click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ShareClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        /// <summary>
        /// The share stream.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void ShareStream(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var data = args.Request.Data;

            if (this.account.IsCompleted && this.account.Result != null)
            {
                data.Properties.Title = string.Format("{0} invited you to {1} on Linkslap!", this.account.Result.UserName, this.viewModel.StreamName);
            }
            else
            {
                data.Properties.Title = string.Format("You were invited to {0} on Linkslap!", this.viewModel.StreamName);
            }

            data.Properties.Description = string.Format("Instantly share pics or pages with your friends!");
            data.SetUri(new Uri(string.Format("http://linkslap.me/s/{0}", this.viewModel.StreamKey), UriKind.Absolute));
        }

        /// <summary>
        /// The settings click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
