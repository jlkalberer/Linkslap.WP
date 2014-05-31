using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Linkslap.WP.Views
{
    using Linkslap.WP.Controls;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View : PageBase
    {
        /// <summary>
        /// The link.
        /// </summary>
        private LinkViewModel link;

        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        public View()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="eventArgs">
        /// The event Args.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            this.link = eventArgs.Parameter as LinkViewModel;

            if (this.link == null)
            {
                // TODO - Go back and show error message
                return;
            }

            this.Title.Text = this.link.Comment;
            this.WebView.Navigate(new Uri(this.link.Url, UriKind.Absolute));

            base.OnNavigatedTo(eventArgs);
        }
    }
}
