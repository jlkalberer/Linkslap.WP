namespace Linkslap.WP.Utils
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// The navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        public void Navigate<TType>(object parameters = null)
        {
            var frame = Window.Current.Content as Frame;
            var page = frame.Content as Page;

            page.Navigate<TType>(parameters);
        }

        /// <summary>
        /// The go back.
        /// </summary>
        public void GoBack()
        {
            var frame = Window.Current.Content as Frame;

            frame.GoBack();
        }

        public void NavigateRoot<TType>(object parameters = null)
        {
            var frame = Window.Current.Content as Frame;
            var page = frame.Content as Page;

            page.NavigateRoot<TType>(parameters);
        }
    }
}
