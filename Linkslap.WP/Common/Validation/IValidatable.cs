namespace Linkslap.WP.Common.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a contract to objects wanting to support data validation.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Gets the validation messages.
        /// </summary>
        /// <value>
        /// The validation messages.
        /// </value>
        Dictionary<string, List<IValidationMessage>> ValidationMessages { get; }

        /// <summary>
        /// Registers an objects properties so that its validation Messages are accessible for observers to access.
        /// </summary>
        /// <param name="propertyName">The name of the property you want to register.</param>
        // void RegisterProperty(params string[] propertyName);

        /// <summary>
        /// Adds the supplied validation message to the ValidationMessages collection.
        /// </summary>
        /// <param name="property">The property this validation was performed against.</param>
        /// <param name="message">The message.</param>
        void AddValidationMessage(string property, IValidationMessage message);

        /// <summary>
        /// Removes the validation message from the ValidationMessages collection.
        /// </summary>
        /// <param name="property">The property this validation was performed against.</param>
        /// <param name="message">The message.</param>
        void RemoveValidationMessage(string property, string message);

        /// <summary>
        /// Removes all of the validation messages associated to the supplied property from the ValidationMessages collection.
        /// </summary>
        /// <param name="property">The property this validation was performed against.</param>
        void RemoveValidationMessages(string property);

        /// <summary>
        /// Determines whether the object has any validation message Type's matching T for the the specified property.
        /// </summary>
        /// <typeparam name="T">A Type implementing IValidationMessage</typeparam>
        /// <param name="property">The property this validation was performed against.</param>
        /// <returns></returns>
        bool HasValidationMessageType<T>(string property) where T : IValidationMessage, new();
    }
}
