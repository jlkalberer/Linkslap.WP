namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.Background;
    using Windows.Data.Xml.Dom;
    using Windows.Networking.PushNotifications;
    using Windows.UI.Notifications;
    using Windows.UI.Popups;

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
        private readonly IAccountStore accountStore;

        /// <summary>
        /// The new slap store.
        /// </summary>
        private readonly INewSlapsStore newSlapStore;

        /// <summary>
        /// The subscription repository.
        /// </summary>
        private readonly ISubscriptionStore subscriptionStore;

        private readonly ISettingsStore settingsStore;

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly HomeViewModel viewModel;

        /// <summary>
        /// Initializes static members of the <see cref="Home"/> class.
        /// </summary>
        static Home()
        {
            var ns = new NotificationStore();
            ns.Register();

            NotificationStore.PushNotificationReceived += (sender, args) => 
                {
                    if (args.RawNotification == null)
                    {
                        return;
                    }

                    var tasK = new PushNotificationTask();
                    tasK.Process(args.RawNotification.Content, false);
                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home()
            : this(new AccountStore(), new NewSlapsStore(), new SubscriptionStore(), new SettingsStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account Store.
        /// </param>
        /// <param name="newSlapStore">
        /// The new Slap Store.
        /// </param>
        /// <param name="subscriptionStore">
        /// The subscription repository.
        /// </param>
        /// <param name="settingsStore">
        /// The settings Store.
        /// </param>
        public Home(IAccountStore accountStore, INewSlapsStore newSlapStore, ISubscriptionStore subscriptionStore, ISettingsStore settingsStore)
        {
            this.accountStore = accountStore;
            this.newSlapStore = newSlapStore;
            this.subscriptionStore = subscriptionStore;
            this.settingsStore = settingsStore;
            this.InitializeComponent();

            this.viewModel = this.DataContext as HomeViewModel;
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

                        if (args.OldItems != null)
                        {
                            args.OldItems.Cast<Subscription>()
                                .Each(s => this.viewModel.Subscriptions.Remove(svm => svm.Id == s.Id));
                        }
                    });
            
            this.DataContext = this.viewModel; // this.Subscriptions;

            this.Pivot.SelectionChanged += this.PivotOnSelectionChanged;

            base.OnNavigatedTo(e);

            //if (!this.newSlapStore.Links.Any())
            //{
            //    this.Pivot.SelectedIndex = 1;
            //}
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
            if (link == null)
            {
                return;
            }

            var linkId = link.Id;
            this.CrossThread(
                () =>
                {
                    var oldLinks = this.viewModel.NewLinks.Where(l => l.Id == linkId).ToList();

                    if (oldLinks.Any())
                    {
                        this.viewModel.NewLinks.RemoveRange(oldLinks);
                    }
                    else if (this.settingsStore.ShowInNewLinks(link.StreamKey))
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
            if (!(sender is Pivot))
            {
                return;
            }

            var pivot = sender as Pivot;

            this.viewModel.PanelHeaderStyles[0] = (Style)Application.Current.Resources["PivotStyle"];
            this.viewModel.PanelHeaderStyles[1] = (Style)Application.Current.Resources["PivotStyle"];

            this.viewModel.PanelHeaderStyles[pivot.SelectedIndex] = (Style)Application.Current.Resources["PivotSelectedStyle"];

            // this.ClearAppBarButton.Visibility = pivot.SelectedIndex == 1 || !this.viewModel.NewLinks.Any() ? Visibility.Collapsed : Visibility.Visible;
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

            ToastNotificationManager.History.Remove(link.Id.ToString());
            // this.newSlapStore.RemoveLink(link.Id);
            this.Navigate<View>(new ViewLinksViewModel(link, new ObservableCollection<LinkViewModel>(this.viewModel.NewLinks)));
        }

        private void RemoveNewSlap(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton))
            {
                return;
            }

            int id = (sender as HyperlinkButton).DataContext is int ? (int)(sender as HyperlinkButton).DataContext : 0;

            if (id == default(int))
            {
                return;
            }

            ToastNotificationManager.History.Remove(id.ToString());
            // this.newSlapStore.RemoveLink(id);
        }

        private void ClearNewSlapsClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.NewLinks.Clear();
            // this.newSlapStore.Clear();
            ToastNotificationManager.History.Clear();
        }

        /// <summary>
        /// The search gif click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SearchGifClick(object sender, RoutedEventArgs e)
        {
            this.Navigate<FindGifs>();
        }

        /// <summary>
        /// The go to settings.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            this.Navigate<Settings>();
        }

        /// <summary>
        /// The logout click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            ToastNotificationManager.History.Clear();
            this.accountStore.Logout();

            this.NavigateRoot<Login>();
        }
    }
}
