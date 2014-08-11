namespace Linkslap.WP.Views
{
    using System;

    using Windows.System;
    using Windows.UI.Xaml;

    using Linkslap.WP.Controls;

    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : PageBase
    {
        public Settings()
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

        private void OpenUrl(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("https://linkslap.me/privacy"));
        }
    }
}
