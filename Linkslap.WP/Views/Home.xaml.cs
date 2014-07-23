namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.Background;
    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

    using AutoMapper;

    using Linkslap.WP.BackgroundTask;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : PageBase
    {
        /// <summary>
        /// The new slap store.
        /// </summary>
        private readonly INewSlapsStore newSlapStore;

        /// <summary>
        /// The subscription repository.
        /// </summary>
        private readonly ISubscriptionStore subscriptionStore;

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly HomeViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home()
            : this(new NewSlapsStore(), new SubscriptionStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="newSlapStore">
        /// The new Slap Store.
        /// </param>
        /// <param name="subscriptionStore">
        /// The subscription repository.
        /// </param>
        public Home(INewSlapsStore newSlapStore, ISubscriptionStore subscriptionStore)
        {
            this.newSlapStore = newSlapStore;
            this.subscriptionStore = subscriptionStore;
            this.InitializeComponent();

            this.viewModel = new HomeViewModel();
        }

        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.MapNewLinks();

            NewSlapsStore.NewSlapsChanged += this.NewSlapsStoreOnNewSlapsChanged;

            var subscriptions = this.subscriptionStore.GetSubsriptions();

            var mappedSubscriptions = new List<SubscriptionViewModel>();
            mappedSubscriptions = Mapper.Map(subscriptions, mappedSubscriptions);

            this.viewModel.Subscriptions.AddRange(mappedSubscriptions);

            subscriptions.CollectionChanged += (sender, args) => this.CrossThread(
                () =>
                    {

                        var newItems = new List<SubscriptionViewModel>();
                        newItems = Mapper.Map(args.NewItems, newItems);
                        this.viewModel.Subscriptions.AddRange(newItems);

                        var oldItems = new List<SubscriptionViewModel>();
                        oldItems = Mapper.Map(args.NewItems, oldItems);
                        this.viewModel.Subscriptions.RemoveRange(oldItems);
                    });

            this.DataContext = this.viewModel; // this.Subscriptions;

            this.Pivot.SelectionChanged += this.PivotOnSelectionChanged;

            base.OnNavigatedTo(e);

            if (!this.newSlapStore.Links.Any())
            {
                this.Pivot.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// The map new links.
        /// </summary>
        private void MapNewLinks()
        {
            var newLinks = this.newSlapStore.Links;
            var mappedLinks = new List<LinkViewModel>();
            mappedLinks = Mapper.Map(newLinks, mappedLinks);
            this.viewModel.NewLinks.AddRange(mappedLinks.OrderByDescending(ml => ml.CreatedDate));
        }

        /// <summary>
        /// The new slaps store on new slaps changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="link">
        /// The link.
        /// </param>
        private void NewSlapsStoreOnNewSlapsChanged(object sender, Link link)
        {
            var linkId = link.Id;
            this.CrossThread(
                () =>
                    {
                        var oldLinks = this.viewModel.NewLinks.Where(l => l.Id == linkId).ToList();

                        if (oldLinks.Any())
                        {
                            this.viewModel.NewLinks.RemoveRange(oldLinks);
                        }
                        else
                        {
                            this.viewModel.NewLinks.Insert(0, Mapper.Map<Link, LinkViewModel>(link));
                        }
                    });
        }

        /// <summary>
        /// The on navigating from.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs eventArgs)
        {
            NewSlapsStore.NewSlapsChanged -= this.NewSlapsStoreOnNewSlapsChanged;

            base.OnNavigatingFrom(eventArgs);
        }

        /// <summary>
        /// The pivot on selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="selectionChangedEventArgs">
        /// The selection changed event args.
        /// </param>
        private void PivotOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var subscription = selectionChangedEventArgs.AddedItems[0] as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }
            //var buttons = ApplicationBar.Buttons.Cast<ApplicationBarIconButton>().ToList();

            //if (subscription.Id == 0)
            //{
            //    buttons[1].IsEnabled = false;
            //    //buttons[2].IsEnabled = false;
            //}
            //else
            //{
            //    buttons[1].IsEnabled = true;
            //    //buttons[2].IsEnabled = true;
            //}
        }

        private void StreamSelectionChanaged(object sender, SelectionChangedEventArgs eventArgs)
        {
            var subscription = eventArgs.AddedItems[0] as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }

            this.Navigate<ViewStream>(subscription);
        }
        
        /// <summary>
        /// The new stream click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void NewStreamClick(object sender, RoutedEventArgs eventArgs)
        {
            this.Navigate<NewStream>();
        }

        private void LinkLongList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // After an item is removed nothing should happen.
            if (e.RemovedItems.Any())
            {
                return;
            }

            var link = e.AddedItems[0] as LinkViewModel;

            if (link == null)
            {
                return;
            }

            this.newSlapStore.RemoveLink(link.Id);
            this.Navigate<View>(link);
        }
    }
}
