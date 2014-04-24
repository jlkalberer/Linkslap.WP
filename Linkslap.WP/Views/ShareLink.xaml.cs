using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Linkslap.WP.Views
{
    using System.Collections.ObjectModel;

    using Windows.ApplicationModel.Core;
    using Windows.ApplicationModel.DataTransfer.ShareTarget;
    using Windows.UI.Core;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareLink : PageBase
    {
        private readonly ISubscriptionStore subscriptionStore;

        private readonly IStreamStore streamStore;

        private ShareOperation shareOperation;

        private Uri data;

        public ShareLink()
            : this(new SubscriptionStore(), new StreamStore())
        {

        }

        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; set; }

        public ShareLink(ISubscriptionStore subscriptionStore, IStreamStore streamStore)
        {
            this.subscriptionStore = subscriptionStore;
            this.streamStore = streamStore;
            this.InitializeComponent();

            this.Subscriptions = new ObservableCollection<SubscriptionViewModel>();
            this.DataContext = this.Subscriptions;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.shareOperation = e.Parameter as ShareOperation;

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
        /// The stream selection chanaged.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StreamSelectionChanaged(object sender, SelectionChangedEventArgs e)
        {
            var subscription = e.AddedItems[0] as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }

            var task = this.streamStore.SlapLink(subscription.StreamKey, "Test", this.data.ToString());

            task.ContinueWith(
                link =>
                    {
                        this.shareOperation.ReportCompleted();
                    });
        }
    }
}
