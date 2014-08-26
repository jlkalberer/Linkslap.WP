namespace Linkslap.WP.ViewModels
{
    using System;

    using AutoMapper;

    using Linkslap.WP.Common.Validation;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Utils;

    using Windows.UI.Popups;

    /// <summary>
    /// The register view model.
    /// </summary>
    public class RegisterViewModel : ValidationBase
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
        /// Initializes a new instance of the <see cref="RegisterViewModel"/> class.
        /// </summary>
        public RegisterViewModel()
            : this(new AccountStore(), new NavigationService())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModel"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account store.
        /// </param>
        /// <param name="navigationService">
        /// The navigation Service.
        /// </param>
        public RegisterViewModel(IAccountStore accountStore, INavigationService navigationService)
        {
            this.accountStore = accountStore;
            this.navigationService = navigationService;
            this.ExecuteButtonEnabled = true;
        }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets the confirm password.
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return this.Password;
            }
        }

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
        /// Validates this instance.
        /// </summary>
        public override void Validate()
        {
            this.ValidateProperty("Email", "email", this.Email)
                    .Required()
                    .Email();

            this.ValidateProperty("UserName", "user name", this.UserName)
                    .Required()
                    .MaxLength(64)
                    .NotEmail();

            this.ValidateProperty("Password", "password", this.Password)
                    .MinLength(6)
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
            return this.ExecuteButtonEnabled;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public override async void Execute(object parameter)
        {
            this.ExecuteButtonEnabled = false;
            base.Execute(parameter);

            if (this.HasErrors)
            {
                this.ExecuteButtonEnabled = true;
                return;
            }

            var model = Mapper.Map<RegisterViewModel, RegisterModel>(this);

            var result = this.accountStore.Register(model);

            await result.ContinueWith(this.ValidateResponse);

            if (result.IsFaulted)
            {
                this.ExecuteButtonEnabled = true;
                this.OnPropertyChanged(string.Empty);
                return;
            }

            var dialog = new MessageDialog("You have successfully registered.  You will receive a confirmation email shortly.");
            await dialog.ShowAsync();

            this.navigationService.GoBack();
        }
    }
}
