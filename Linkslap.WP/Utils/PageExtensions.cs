namespace Linkslap.WP.Utils
{
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using Linkslap.WP.Controls;

    /// <summary>
    /// The page extensions.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// The navigate.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="TType">
        /// </typeparam>
        public static void Navigate<TType>(this Page page, object parameters = null)
        {
            page.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                    {
                        var type = typeof(TType);
                        if (parameters == null)
                        {
                            page.Frame.Navigate(type);
                        }
                        else
                        {
                            page.Frame.Navigate(type, parameters);
                        }
                    });
        }
    }
}
