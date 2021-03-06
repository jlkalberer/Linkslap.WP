﻿namespace Linkslap.WP.Views
{
    using System;
    using System.Linq;

    using Windows.System;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Windows.UI.Notifications;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FindGifs : PageBase
    {
        /// <summary>
        /// The account store.
        /// </summary>
        private readonly IAccountStore accountStore;

        /// <summary>
        /// The stream key.
        /// </summary>
        private string streamKey;

        private readonly FindGifViewModel viewModel;

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

            this.viewModel = this.DataContext as FindGifViewModel;
        }

        /// <summary>
        /// Gets or sets a value indicating whether has viewed search how to.
        /// </summary>
        public static bool HasViewedSearchHowTo
        {
            get
            {
                return Storage.Load<bool>("NewUser.GifSearch");
            }

            set
            {
                Storage.Save("NewUser.GifSearch", value);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.streamKey = (string)e.Parameter;
            base.OnNavigatedTo(e);

            if (!HasViewedSearchHowTo)
            {
                var mess =
                    new MessageDialog(
                        "This is the gif search.\r\n\r\nUse the search box to search.\r\n\r\nTap an image once to preview.\r\n\r\nClick the share icon or double tap to share.");
                await mess.ShowAsync();
                HasViewedSearchHowTo = true;
            }
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
            if (!(e.OriginalSource is Image))
            {
                return;
            }

            var gifViewModel = (GifViewModel)((Image)e.OriginalSource).DataContext;
            gifViewModel.StreamKey = this.streamKey;

            this.Navigate<ShareLink>(gifViewModel);
        }

        private void ItemTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!(e.OriginalSource is Image))
            {
                return;
            }

            var gifViewModel = (GifViewModel)((Image)e.OriginalSource).DataContext;
            gifViewModel.StreamKey = this.streamKey;

            this.Navigate<ShareLink>(gifViewModel);
        }

        private void ShareButtonClicked(object sender, RoutedEventArgs e)
        {
            var gifViewModel = (GifViewModel)((HyperlinkButton)e.OriginalSource).DataContext;
            gifViewModel.StreamKey = this.streamKey;

            this.Navigate<ShareLink>(gifViewModel);
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

        private void SearchTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
            {
                return;
            }

            var textBox = sender as TextBox;

            if (textBox == null)
            {
                return;
            }

            this.viewModel.Query = textBox.Text;
            if (!this.viewModel.CanExecute(null))
            {
                return;
            }

            textBox.IsEnabled = false;
            textBox.IsEnabled = true;
            this.viewModel.Execute(null);
        }
    }
}
