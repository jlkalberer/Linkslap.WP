namespace Linkslap.WP.ViewModels
{
    using Linkslap.WP.Common.Validation;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Utils;
    using Linkslap.WP.Views;

    /// <summary>
    /// The login view model.
    /// </summary>
    public class LoginViewModel : ValidationBase
    {
        /// <summary>
        /// The account store.
        /// </summary>
        private readonly IAccountStore accountStore;

        private readonly INavigationService navigationService;

        /// <summary>
        /// The running request.
        /// </summary>
        private bool runningRequest;

        public LoginViewModel()
            : this(new AccountStore(), new NavigationService())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account store.
        /// </param>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        public LoginViewModel(IAccountStore accountStore, INavigationService navigationService)
        {
            this.accountStore = accountStore;
            this.navigationService = navigationService;
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The validate.
        /// </summary>
        public override void Validate()
        {
            this.ValidateProperty("UserName", "user name", this.UserName)
                .Required();

            this.ValidateProperty("Password", "password", this.Password)
                .Required();

            base.Validate();
        }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return !this.runningRequest;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public async override void Execute(object parameter)
        {
            base.Execute(parameter);

            if (this.HasErrors)
            {
                return;
            }

            this.runningRequest = true;
            var result = this.accountStore.Authenticate(this.UserName, this.Password);

            await result.ContinueWith(this.ValidateResponse);

            if (result.IsFaulted)
            {
                this.runningRequest = false;
                this.OnPropertyChanged(string.Empty);
                return;
            }

            var ns = new NotificationStore();
            ns.Register();

            this.navigationService.NavigateRoot<Home>();
        }
    }
}
