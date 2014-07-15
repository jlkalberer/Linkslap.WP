using System.Linq;

namespace Linkslap.WP.ViewModels
{
    using Linkslap.WP.Validation;

    /// <summary>
    /// The register view model.
    /// </summary>
    public class RegisterViewModel : ValidationBase
    {
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
        /// Gets or sets ConfirmPassword.
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// The valid.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Valid()
        {
            this.ValidateMember("Email", this.Email)
                .Required()
                .Email();

            this.ValidateMember("user name", this.UserName)
                .Required()
                .MaxLength(64)
                .NotEmail();

            this.ValidateMember("password", this.Password)
                .Required();

            this.ValidateMember("confirmation password", this.ConfirmPassword)
                .Required()
                .Compare(this.Password, "The password and {0} do not match");

            return this.Errors.Any();
        }
    }
}
