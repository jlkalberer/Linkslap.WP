namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
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

        private readonly HomeViewModel viewModel;

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

            this.NewSlaps = new SubscriptionViewModel { Name = "New Slaps" };
            this.Subscriptions = new ObservableCollection<SubscriptionViewModel> { this.NewSlaps };

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

            this.viewModel = new HomeViewModel();
            this.viewModel.NewLinks.Add(new LinkViewModel { Title = "foobar" });

            this.DataContext = this.viewModel; // this.Subscriptions;

            this.Pivot.SelectionChanged += this.PivotOnSelectionChanged;
        }

        /// <summary>
        /// Gets or sets the new slaps.
        /// </summary>
        public SubscriptionViewModel NewSlaps { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions { get; set; }

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

            var buttons = ApplicationBar.Buttons.Cast<ApplicationBarIconButton>().ToList();

            if (subscription.Id == 0)
            {
                buttons[1].IsEnabled = false;
                //buttons[2].IsEnabled = false;
            }
            else
            {
                buttons[1].IsEnabled = true;
                //buttons[2].IsEnabled = true;
            }
        }

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
        /// Navigates to a view for sharing a stream.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event arguments.
        /// </param>
        private void ShareStream_Click(object sender, EventArgs eventArgs)
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

        private void DeleteSubscription_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}