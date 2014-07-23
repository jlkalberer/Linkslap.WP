namespace Linkslap.WP.Views
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.Background;
    using Windows.Networking.PushNotifications;

    using Linkslap.WP.BackgroundTask;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;

    using Windows.UI.Xaml;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : PageBase
    {
        /// <summary>
        /// The account repository.
        /// </summary>
        private readonly IAccountStore accountStore;

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
        }

        /// <summary>
        /// The login button_ on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.LoginButton.IsEnabled = false;
            if (string.IsNullOrEmpty(this.UserName.Text))
            {
                this.LoginButton.IsEnabled = true;
                return;
            }

            if (string.IsNullOrEmpty(this.Password.Password))
            {
                this.LoginButton.IsEnabled = true;
                return;
            }

            var task = this.accountStore.Authenticate(this.UserName.Text, this.Password.Password);

            task.ContinueWith(async account =>
                {
                    if (account == null || account.Status != TaskStatus.RanToCompletion)
                    {
                        return;
                    }

                    this.Navigate<Home>();
                });
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
    }
}
