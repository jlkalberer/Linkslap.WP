﻿namespace Linkslap.WP.Views
{
    using Windows.UI.Notifications;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Navigation;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewStream : PageBase
    {
        /// <summary>
        /// The stream store.
        /// </summary>
        private readonly IStreamStore streamStore;

        private readonly AccountStore accountStore;

        private int errorCount = 0;

        private NewSlapStreamViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewStream"/> class.
        /// </summary>
        public NewStream()
            : this(new StreamStore(), new AccountStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewStream"/> class.
        /// </summary>
        /// <param name="streamStore">
        /// The stream Store.
        /// </param>
        /// <param name="accountStore">
        /// The account Store.
        /// </param>
        public NewStream(IStreamStore streamStore, AccountStore accountStore)
        {
            this.streamStore = streamStore;
            this.accountStore = accountStore;
            this.InitializeComponent();

            this.viewModel = this.DataContext as NewSlapStreamViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// The create button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void CreateButtonClick(object sender, RoutedEventArgs eventArgs)
        {
            this.viewModel.CanSubmit = false;
            this.streamStore.NewStream(this.viewModel.StreamName).ContinueWith(
                task =>
                    {
                        if (!task.IsCompleted)
                        {
                            // An error occurred. We should log this crap here..
                            errorCount++;

                            if (errorCount >= 3)
                            {
                                this.NavigationHelper.GoBack();
                            }

                            this.CreateButton.IsEnabled = false;
                        }

                        this.NavigationHelper.GoBack();
                    });
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
