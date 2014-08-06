namespace Linkslap.WP.Views
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PasswordReset : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordReset"/> class.
        /// </summary>
        public PasswordReset()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The textbox focused.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextboxFocused(object sender, RoutedEventArgs e)
        {
            this.CommandBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The text box lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            this.CommandBar.Visibility = Visibility.Collapsed;
        }
    }
}
