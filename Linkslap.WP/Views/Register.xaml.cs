namespace Linkslap.WP.Views
{
    using Windows.System;
    using Windows.UI.Xaml.Input;

    using Linkslap.WP.Controls;

    using Windows.UI.Xaml;

    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : PageBase
    {
        private RegisterViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Register"/> class.
        /// </summary>
        public Register()
        {
            this.InitializeComponent();

            this.viewModel = this.DataContext as RegisterViewModel;
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

        private void UsernameKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
            {
                return;
            }

            this.Password.Focus(FocusState.Programmatic);
        }

        private void PasswordKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
            {
                return;
            }

            this.Email.Focus(FocusState.Programmatic);
        }

        private void EmailKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
            {
                return;
            }

            this.viewModel.Email = this.Email.Text;
            if (!this.viewModel.CanExecute(null))
            {
                return;
            }

            this.Email.IsEnabled = false;
            this.Email.IsEnabled = true;
            this.viewModel.Execute(null);
        }
    }
}
