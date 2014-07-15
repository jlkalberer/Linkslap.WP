namespace Linkslap.WP.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The validation base.
    /// </summary>
    public abstract class ValidationBase : IModelValidation
    {
        private List<Tuple<string, string>> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBase"/> class.
        /// </summary>
        public ValidationBase()
        {
            this.errors = new List<Tuple<string, string>>();
        }

        public IEnumerable<Tuple<string, string>> Errors
        {
            get
            {
                return this.errors;
            }
        }

        public abstract bool Valid();

        /// <summary>
        /// The validate member.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="TValue">
        /// The value type to validate.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Validator"/>.
        /// </returns>
        protected Validator<TValue> ValidateMember<TValue>(string name, TValue value) where TValue : class
        {
            return new Validator<TValue>(name, value, this.errors);
        }

        /// <summary>
        /// The validator.
        /// </summary>
        /// <typeparam name="TValue">
        /// </typeparam>
        protected sealed class Validator<TValue> where TValue : class 
        {
            /// <summary>
            /// The name.
            /// </summary>
            private readonly string name;

            /// <summary>
            /// The value.
            /// </summary>
            private readonly TValue value;

            /// <summary>
            /// The errors.
            /// </summary>
            private readonly IList<Tuple<string, string>> errors;

            /// <summary>
            /// Initializes a new instance of the <see cref="Validator"/> class.
            /// </summary>
            /// <param name="name">
            /// The name.
            /// </param>
            /// <param name="value">
            /// The value.
            /// </param>
            /// <param name="errors">
            /// The errors.
            /// </param>
            public Validator(string name, TValue value, IList<Tuple<string, string>> errors)
            {
                this.name = name;
                this.value = value;
                this.errors = errors;
            }

            /// <summary>
            /// The required.
            /// </summary>
            /// <returns>
            /// The <see cref="Validator"/>.
            /// </returns>
            public Validator<TValue> Required()
            {
                if (this.value == default(TValue))
                {
                    this.AddError("{0} is required.");
                }

                return this;
            }

            /// <summary>
            /// The max length.
            /// </summary>
            /// <param name="length">
            /// The length.
            /// </param>
            /// <returns>
            /// The <see cref="Validator"/>.
            /// </returns>
            public Validator<string> MaxLength(int length)
            {
                var s = this.value as string;
                if (s != null && s.Length > length)
                {
                    this.AddError("{0} must be shorter than " + length + " characters.");
                }

                return this as Validator<string>;
            }

            /// <summary>
            /// The min length.
            /// </summary>
            /// <param name="length">
            /// The length.
            /// </param>
            /// <returns>
            /// The <see cref="Validator"/>.
            /// </returns>
            public Validator<string> MinLength(int length)
            {
                var s = this.value as string;
                if (s != null && s.Length < length)
                {
                    this.AddError("The {0} must be at least " + length + " characters long.");
                }

                return this as Validator<string>;
            }

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            /// <param name="errorMessage">
            /// The error message.
            /// </param>
            /// <returns>
            /// The <see cref="Validator"/>.
            /// </returns>
            public Validator<TValue> Compare(TValue value, string errorMessage)
            {
                if (this.value != value)
                {
                    this.AddError(errorMessage);
                }

                return this;
            }

            /// <summary>
            /// The email.
            /// </summary>
            /// <returns>
            /// The <see cref="Validator"/>.
            /// </returns>
            public Validator<TValue> Email()
            {
                // validate email with regex

                return this;
            }

            /// <summary>
            /// The not email.
            /// </summary>
            /// <returns>
            /// The <see cref="Validator"/>.
            /// </returns>
            public Validator<TValue> NotEmail()
            {
                // validate not email

                return this;
            }

            /// <summary>
            /// The add error.
            /// </summary>
            /// <param name="formattedMessage">
            /// The formatted message.
            /// </param>
            private void AddError(string formattedMessage)
            {
                this.errors.Add(new Tuple<string, string>(this.name, string.Format(formattedMessage, this.name)));
            }
        }
    }
}
