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

                    var stack = page.Frame.BackStack;

                    if (stack.Any(s => s.SourcePageType == type))
                    {
                        while (stack.Any() && stack.Last().SourcePageType != type)
                        {
                            stack.RemoveAt(stack.Count - 1);
                        }

                        stack.RemoveAt(stack.Count - 1);
                    }
                });
        }

        public static void NavigateRemoveFrames<TType, TRemoveType>(this Page page, object parameters = null)
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

                    var stack = page.Frame.BackStack;

                    var removeType = typeof(TRemoveType);

                    if (stack.Any(s => s.SourcePageType == removeType))
                    {
                        while (stack.Any() && stack.Last().SourcePageType != removeType)
                        {
                            stack.RemoveAt(stack.Count - 1);
                        }

                        stack.RemoveAt(stack.Count - 1);
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

                    var stack = page.Frame.BackStack;
                    while (stack.Count > 1)
                    {
                        stack.RemoveAt(stack.Count - 1);
                    }
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
