namespace Linkslap.WP.Utils
{
    using System;
    using System.Windows;

    using Microsoft.Phone.Controls;

    /// <summary>
    /// The phone application page helper.
    /// </summary>
    public static class PhoneApplicationPageHelper
    {
        /// <summary>
        /// The navigate.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageUrl">
        /// The page url.
        /// </param>
        /// <param name="uriKind">
        /// The uri kind.
        /// </param>
        public static void Navigate(this PhoneApplicationPage page, string pageUrl, UriKind uriKind = UriKind.Relative)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () => page.NavigationService.Navigate(new Uri(pageUrl, uriKind)));
        }
    }
}
