namespace Linkslap.WP.Views
{
    using System.Collections.Generic;

    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : PageBase
    {
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
            : this(new SubscriptionStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="subscriptionStore">
        /// The subscription repository.
        /// </param>
        public Home(ISubscriptionStore subscriptionStore)
        {
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var subscriptions = this.subscriptionStore.GetSubsriptions();

            var mappedSubscriptions = new List<SubscriptionViewModel>();
            mappedSubscriptions = Mapper.Map(subscriptions, mappedSubscriptions);

            this.viewModel.Subscriptions.AddRange(mappedSubscriptions);

            subscriptions.CollectionChanged += (sender, args) =>
            {
                var newItems = new List<SubscriptionViewModel>();
                newItems = Mapper.Map(args.NewItems, newItems);
                this.viewModel.Subscriptions.AddRange(newItems);

                var oldItems = new List<SubscriptionViewModel>();
                oldItems = Mapper.Map(args.NewItems, oldItems);
                this.viewModel.Subscriptions.RemoveRange(oldItems);
            };

            this.DataContext = this.viewModel; // this.Subscriptions;

            this.Pivot.SelectionChanged += this.PivotOnSelectionChanged;

            base.OnNavigatedTo(e);
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
    }
}
