﻿using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Linkslap.WP.Views
{
    using Windows.UI.Notifications;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FindGifs : PageBase
    {
        /// <summary>
        /// The account store.
        /// </summary>
        private readonly IAccountStore accountStore;

        private string streamKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindGifs"/> class.
        /// </summary>
        public FindGifs()
            : this(new AccountStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindGifs"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account store.
        /// </param>
        public FindGifs(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.streamKey = (string)e.Parameter;
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// The image clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ImageClicked(object sender, SelectionChangedEventArgs e)
        {
            var item = (GifViewModel)e.AddedItems.FirstOrDefault();

            if (item == null)
            {
                return;
            }

            item.ShowGif = true;

            item = (GifViewModel)e.RemovedItems.FirstOrDefault();

            if (item == null)
            {
                return;
            }

            item.ShowGif = false;
        }

        /// <summary>
        /// The image opened.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ImageOpened(object sender, RoutedEventArgs e)
        {
            ((Image)sender).Height = ((Image)sender).ActualHeight;
        }

        /// <summary>
        /// The item held.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ItemHeld(object sender, HoldingRoutedEventArgs e)
        {
            this.Navigate<ShareLink>(((Image)e.OriginalSource).DataContext);
        }

        /// <summary>
        /// The go home.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GoHome(object sender, RoutedEventArgs e)
        {
            this.NavigateRoot<Home>();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            this.Navigate<SettingsFlyout>();
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            ToastNotificationManager.History.Clear();
            this.accountStore.Logout();

            this.NavigateRoot<Login>();
        }
    }
}
