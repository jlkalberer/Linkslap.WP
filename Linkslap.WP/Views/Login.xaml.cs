namespace Linkslap.WP.Views
{
    using System.Threading.Tasks;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;

    using Windows.UI.Xaml;

    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : PageBase
    {
        /// <summary>
        /// The account repository.
        /// </summary>
        private readonly IAccountStore accountStore;

        private LoginViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
            : this(new AccountStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account repository.
        /// </param>
        public Login(IAccountStore accountStore)
        {
            this.accountStore = accountStore;
            this.InitializeComponent();

            this.viewModel = this.DataContext as LoginViewModel;
        }
        
        /// <summary>
        /// The register button_ on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Navigate<Register>();
        }

        private void TextboxFocused(object sender, RoutedEventArgs e)
        {
            this.CommandBar.Visibility = Visibility.Visible;
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            this.CommandBar.Visibility = Visibility.Collapsed;
        }

        private void ForgotPasswordClick(object sender, RoutedEventArgs e)
        {
            this.Navigate<PasswordReset>();
        }
    }
}
