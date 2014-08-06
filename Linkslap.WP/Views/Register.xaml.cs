﻿namespace Linkslap.WP.Views
{
    using Linkslap.WP.Controls;

    using Windows.UI.Xaml;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : PageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Register"/> class.
        /// </summary>
        public Register()
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
