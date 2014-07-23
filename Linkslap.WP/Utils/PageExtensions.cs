namespace Linkslap.WP.Utils
{
    using System.Linq;

    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;

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
            CrossThread(
                page,
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

        public static void NavigateRoot<TType>(this Page page, object parameters = null)
        {
            CrossThread(
                page,
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

                    page.Frame.BackStack.Clear();
                });
        }

        /// <summary>
        /// The cross thread.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public static void CrossThread(this Page page, DispatchedHandler action)
        {
            if (action == null)
            {
                return;
            }

            page.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }
    }
}
