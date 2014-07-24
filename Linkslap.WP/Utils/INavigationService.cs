namespace Linkslap.WP.Utils
{
    /// <summary>
    /// The NavigationService interface.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// The navigate.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="TType">
        /// </typeparam>
        void Navigate<TType>(object parameters = null);

        /// <summary>
        /// The go back.
        /// </summary>
        void GoBack();

        /// <summary>
        /// The navigate root.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="TType">
        /// The type of view to navigate to.
        /// </typeparam>
        void NavigateRoot<TType>(object parameters = null);
    }
}
