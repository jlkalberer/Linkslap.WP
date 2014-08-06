using System;
using System.Collections.Generic;
using System.Linq;

namespace Linkslap.WP.Common.Validation
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Linkslap.WP.Annotations;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// An implementation of IValidatable and INotifyPropertyChanged for validation and property change tracking.
    /// </summary>
    public abstract class ValidationBase : IValidatable, INotifyPropertyChanged, ICommand
    {
        /// <summary>
        /// The generic error.
        /// </summary>
        private const string GenericError = "GenericError";

        /// <summary>
        /// The email regex.
        /// </summary>
        private static Regex emailRegex = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        /// <summary>
        /// The ValidationMessages backing field.
        /// </summary>
        private Dictionary<string, List<IValidationMessage>> validationMessages = new Dictionary<string, List<IValidationMessage>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBase"/> class.
        /// </summary>
        public ValidationBase()
        {
            this.validationMessages[GenericError] = new List<IValidationMessage>();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Gets the validation messages.
        /// </summary>
        /// <value>
        /// The validation messages.
        /// </value>
        public Dictionary<string, List<IValidationMessage>> ValidationMessages
        {
            get
            {
                return this.validationMessages;
            }

            private set
            {
                this.validationMessages = value;
                this.OnPropertyChanged("ValidationMessages");
            }
        }

        /// <summary>
        /// Gets a value indicating whether has errors.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return this.validationMessages.SelectMany(v => v.Value).Any();
            }
        }

        /// <summary>
        /// Registers an objects properties its validation Messages are accessible for observers to access.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        public void RegisterProperty(params string[] propertyNames)
        {
            foreach (string property in propertyNames)
            {
                if (!this.ValidationMessages.ContainsKey(property))
                {
                    this.ValidationMessages[property] = new List<IValidationMessage>();
                }
            }
        }

        /// <summary>
        /// Determines whether the object has any validation message Type's matching T for the the specified property.
        /// </summary>
        /// <typeparam name="T">A Type implementing IValidationMessage</typeparam>
        /// <param name="property">The property this validation was performed against.</param>
        /// <returns></returns>
        public bool HasValidationMessageType<T>(string property) where T : IValidationMessage, new()
        {
            if (string.IsNullOrEmpty(property))
            {
                bool result = this.validationMessages.Values.Any(collection => collection.Any(msg => msg is T));
                return result;
            }

            return this.validationMessages.ContainsKey(property);
        }

        /// <summary>
        /// Adds the supplied validation message to the ValidationMessages collection.
        /// </summary>
        /// <param name="property">
        /// The property this validation was performed against.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void AddValidationMessage(string property, IValidationMessage message)
        {
            // If the key does not exist, then we create one.
            if (!this.validationMessages.ContainsKey(property))
            {
                this.validationMessages[property] = new List<IValidationMessage>();
            }

            if (this.validationMessages[property].Any(msg => msg.Message.Equals(message.Message) || msg == message))
            {
                return;
            }

            this.validationMessages[property].Add(message);
        }

        /// <summary>
        /// Removes the validation message from the ValidationMessages collection.
        /// </summary>
        /// <param name="property">
        /// The property this validation was performed against.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void RemoveValidationMessage(string property, string message)
        {
            if (!this.validationMessages.ContainsKey(property))
            {
                return;
            }

            if (this.validationMessages[property].Any(msg => msg.Message.Equals(message)))
            {
                // Remove the error from the key's collection.
                this.validationMessages[property].Remove(
                    this.validationMessages[property].FirstOrDefault(msg => msg.Message.Equals(message)));
            }
        }

        /// <summary>
        /// Removes all of the validation messages associated to the supplied property from the ValidationMessages collection.
        /// </summary>
        /// <param name="property">The property this validation was performed against.</param>
        public void RemoveValidationMessages(string property)
        {
            if (!this.validationMessages.ContainsKey(property))
            {
                return;
            }

            this.validationMessages[property].Clear();
            this.validationMessages.Remove(property);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public virtual void Validate()
        {
            this.OnPropertyChanged(string.Empty);
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
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public virtual void Execute(object parameter)
        {
            this.validationMessages[GenericError] = new List<IValidationMessage>();
            this.Validate();
        }

        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <typeparam name="TType">
        /// </typeparam>
        /// <param name="propertyName">
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// Returns a validation message if the validation failed. Otherwise, null is returned.
        /// </returns>
        protected Validator<TType> ValidateProperty<TType>(string propertyName, string displayName, TType value) where TType : class
        {
            this.validationMessages[propertyName] = new List<IValidationMessage>();

            return new Validator<TType>(propertyName, displayName, value, this);
        }

        /// <summary>
        /// The validate response.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        protected void ValidateResponse(Task task)
        {
            if (task.Status != TaskStatus.Faulted)
            {
                return;
            }

            var errorMessage = task.Exception.InnerExceptions.FirstOrDefault(ie => ie is HttpError) as HttpError;

            if (errorMessage == null)
            {
                return;
            }

            if (errorMessage.ModelState == null)
            {
                if (!string.IsNullOrEmpty(errorMessage.ErrorMessage))
                {
                    this.AddValidationMessage(GenericError, new ValidationErrorMessage(errorMessage.ErrorMessage));
                }

                return;
            }

            foreach (var kvp in errorMessage.ModelState)
            {
                foreach (var error in kvp.Value)
                {
                    var key = !string.IsNullOrEmpty(kvp.Key) ? kvp.Key : GenericError;
                    this.AddValidationMessage(key, new ValidationErrorMessage(error));
                }
            }
        }

        /// <summary>
        /// Called when the specified property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The validator.
        /// </summary>
        /// <typeparam name="TValue">
        /// The value type of the property.
        /// </typeparam>
        protected sealed class Validator<TValue> where TValue : class
        {
            /// <summary>
            /// The name.
            /// </summary>
            private readonly string name;

            /// <summary>
            /// The display name.
            /// </summary>
            private readonly string displayName;

            /// <summary>
            /// The value.
            /// </summary>
            private readonly TValue value;

            /// <summary>
            /// The validation base.
            /// </summary>
            private readonly ValidationBase validationBase;

            /// <summary>
            /// Initializes a new instance of the <see cref="Validator{TValue}"/> class. 
            /// </summary>
            /// <param name="name">
            /// The name.
            /// </param>
            /// <param name="displayName">
            /// The display Name.
            /// </param>
            /// <param name="value">
            /// The value.
            /// </param>
            /// <param name="validationBase">
            /// The validation Base.
            /// </param>
            public Validator(string name, string displayName, TValue value, ValidationBase validationBase)
            {
                this.name = name;
                this.displayName = displayName;
                this.value = value;
                this.validationBase = validationBase;
            }

            /// <summary>
            /// The custom.
            /// </summary>
            /// <param name="failureMessage">
            /// The failure message.
            /// </param>
            /// <param name="validationDelegate">
            /// The validation delegate.
            /// </param>
            /// <returns>
            /// The <see cref="IValidationMessage"/>.
            /// </returns>
            public IValidationMessage CustomValidation(string failureMessage, Func<string, IValidationMessage> validationDelegate)
            {
                IValidationMessage result = validationDelegate(failureMessage);
                if (result != null)
                {
                    this.validationBase.AddValidationMessage(this.name, result);
                }
                else
                {
                    this.validationBase.RemoveValidationMessage(this.name, failureMessage);
                }

                return result;
            }

            /// <summary>
            /// The required.
            /// </summary>
            /// <returns>
            /// The <see cref="Validator{TValue}"/>.
            /// </returns>
            public Validator<TValue> Required()
            {
                if (this.value == default(TValue))
                {
                    this.AddError("{0} is required.");
                }
                else if (this.value is string && string.IsNullOrWhiteSpace((this.value as string)))
                {
                    this.AddError("{0} is required");
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
            /// The <see cref="Validator{TValue}"/>.
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
            /// The <see cref="Validator{TValue}"/>.
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
            /// The <see cref="Validator{TValue}"/>.
            /// </returns>
            public Validator<TValue> Compare(TValue value, string errorMessage)
            {
                if (value is string)
                {
                    if (string.CompareOrdinal(value as string, this.value as string) != 0)
                    {
                        this.AddError(errorMessage);
                    }
                }
                else
                {
                    if (this.value != value)
                    {
                        this.AddError(errorMessage);
                    }
                }

                return this;
            }

            /// <summary>
            /// The email.
            /// </summary>
            /// <returns>
            /// The <see cref="Validator{TValue}"/>.
            /// </returns>
            public Validator<string> Email()
            {
                if (this.value != null && !emailRegex.IsMatch(this.value as string))
                {
                    this.AddError("The {0} field must be a valid email address.");
                }

                return this as Validator<string>;
            }

            /// <summary>
            /// The not email.
            /// </summary>
            /// <returns>
            /// The <see cref="Validator{TValue}"/>.
            /// </returns>
            public Validator<string> NotEmail()
            {
                if (this.value != null && emailRegex.IsMatch(this.value as string))
                {
                    this.AddError("The {0} field must not be an email address.");
                }

                return this as Validator<string>;
            }

            /// <summary>
            /// The add error.
            /// </summary>
            /// <param name="errorMessage">
            /// The error message.
            /// </param>
            private void AddError(string errorMessage)
            {
                var lowerCase = string.Format(errorMessage, this.displayName).ToLower();
                var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
                var result = r.Replace(lowerCase, s => s.Value.ToUpper());

                this.validationBase.AddValidationMessage(
                    this.name,
                    new ValidationErrorMessage(result));
            }
        }
    }
}
