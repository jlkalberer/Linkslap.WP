namespace Linkslap.WP.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The ModelValidation interface.
    /// </summary>
    public interface IModelValidation
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        IEnumerable<Tuple<string, string>> Errors { get; }

        /// <summary>
        /// The valid.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Valid();
    }
}
