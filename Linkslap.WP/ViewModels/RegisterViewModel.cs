namespace Linkslap.WP.ViewModels
{
    using Linkslap.WP.Common.Validation;

    /// <summary>
    /// The register view model.
    /// </summary>
    public class RegisterViewModel : ValidationBase
    {
        private string email;

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;

                this.OnPropertyChanged("Email");
            }
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets ConfirmPassword.
        /// </summary>
        public string ConfirmPassword { get; set; }

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
                    .Required();

            this.ValidateProperty("ConfirmPassword", "confirmation password", this.ConfirmPassword)
                    .Required()
                    .Compare(this.Password, "The password and {0} do not match");

            base.Validate();
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            if (this.HasErrors)
            {
                return;
            }


        }
    }
}
