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
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FindGifs : PageBase
    {
        public FindGifs()
        {
            this.InitializeComponent();
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
    }
}
