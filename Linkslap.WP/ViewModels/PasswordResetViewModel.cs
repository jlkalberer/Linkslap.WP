namespace Linkslap.WP.ViewModels
{
    using System;

    using Windows.UI.Popups;

    using Linkslap.WP.Common.Validation;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Utils;
    using Linkslap.WP.Views;

    /// <summary>
    /// The password reset view model.
    /// </summary>
    public class PasswordResetViewModel : ValidationBase
    {
        /// <summary>
        /// The account store.
        /// </summary>
        private readonly IAccountStore accountStore;

        /// <summary>
        /// The navigation service.
        /// </summary>
        private readonly INavigationService navigationService;

        /// <summary>
        /// The execute button enabled.
        /// </summary>
        private bool executeButtonEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetViewModel"/> class.
        /// </summary>
        public PasswordResetViewModel()
            : this(new AccountStore(), new NavigationService())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetViewModel"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account Store.
        /// </param>
        /// <param name="navigationService">
        /// The navigation Service.
        /// </param>
        public PasswordResetViewModel(IAccountStore accountStore, INavigationService navigationService)
        {
            this.accountStore = accountStore;
            this.navigationService = navigationService;
            this.ExecuteButtonEnabled = true;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether execute button enabled.
        /// </summary>
        public bool ExecuteButtonEnabled
        {
            get
            {
                return this.executeButtonEnabled;
            }

            set
            {
                this.executeButtonEnabled = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The validate.
        /// </summary>
        public override void Validate()
        {
            this.ValidateProperty("Email", "Email", this.Email)
                .Required()
                .Email();

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
            return this.ExecuteButtonEnabled;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public async override void Execute(object parameter)
        {
            this.ExecuteButtonEnabled = false;
            base.Execute(parameter);

            if (this.HasErrors)
            {
                this.ExecuteButtonEnabled = true;
                return;
            }

            var result = this.accountStore.ResetPassword(this.Email);

            await result.ContinueWith(this.ValidateResponse);

            if (result.IsFaulted)
            {
                this.ExecuteButtonEnabled = true;
                this.OnPropertyChanged(string.Empty);
                return;
            }

            var dialog = new MessageDialog("An email has been sent to your address.  Follow the instructions to reset your password.");
            await dialog.ShowAsync();

            this.navigationService.NavigateRoot<Login>();
        }
    }
}