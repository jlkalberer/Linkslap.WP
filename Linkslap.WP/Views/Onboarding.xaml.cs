namespace Linkslap.WP.Views
{
    using Windows.UI.Xaml;

    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Onboarding : PageBase
    {
        public Onboarding()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The ok button clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            Storage.Save("NewUser.Onboarding", true);
            this.NavigationHelper.GoBack();
        }
    }
}
