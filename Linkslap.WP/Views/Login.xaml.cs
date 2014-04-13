namespace Linkslap.WP.Views
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Utils;

    using Microsoft.Phone.Controls;

    /// <summary>
    /// The login view.
    /// </summary>
    public partial class Login : PhoneApplicationPage
    {
        private readonly IAccountRepository accountRepository;

        public Login()
            : this(new AccountRepository())
        {
        }

        public Login(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
            InitializeComponent();
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.UserName.Text))
            {
                return;
            }

            if (string.IsNullOrEmpty(this.Password.Password))
            {
                return;
            }

            var task = this.accountRepository.Authenticate(this.UserName.Text, this.Password.Password);

            task.ContinueWith(
                account =>
                    {
                        if (account != null)
                        {
                            this.Navigate("/Views/Home.xaml");
                        }
                    });
        }
    }
}