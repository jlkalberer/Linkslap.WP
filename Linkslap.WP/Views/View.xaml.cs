namespace Linkslap.WP.Views
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Windows.ApplicationModel.DataTransfer;
    using Windows.Phone.UI.Input;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Windows.UI.Notifications;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View : PageBase
    {
        private readonly INewSlapsStore newSlapsStore;

        private readonly AccountStore accountStore;

        private const string ImageCss =
            "background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKAAAACgCAMAAAC8EZcfAAAKOWlDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAEjHnZZ3VFTXFofPvXd6oc0wAlKG3rvAANJ7k15FYZgZYCgDDjM0sSGiAhFFRJoiSFDEgNFQJFZEsRAUVLAHJAgoMRhFVCxvRtaLrqy89/Ly++Osb+2z97n77L3PWhcAkqcvl5cGSwGQyhPwgzyc6RGRUXTsAIABHmCAKQBMVka6X7B7CBDJy82FniFyAl8EAfB6WLwCcNPQM4BOB/+fpFnpfIHomAARm7M5GSwRF4g4JUuQLrbPipgalyxmGCVmvihBEcuJOWGRDT77LLKjmNmpPLaIxTmns1PZYu4V8bZMIUfEiK+ICzO5nCwR3xKxRoowlSviN+LYVA4zAwAUSWwXcFiJIjYRMYkfEuQi4uUA4EgJX3HcVyzgZAvEl3JJS8/hcxMSBXQdli7d1NqaQffkZKVwBALDACYrmcln013SUtOZvBwAFu/8WTLi2tJFRbY0tba0NDQzMv2qUP91829K3NtFehn4uWcQrf+L7a/80hoAYMyJarPziy2uCoDOLQDI3fti0zgAgKSobx3Xv7oPTTwviQJBuo2xcVZWlhGXwzISF/QP/U+Hv6GvvmckPu6P8tBdOfFMYYqALq4bKy0lTcinZ6QzWRy64Z+H+B8H/nUeBkGceA6fwxNFhImmjMtLELWbx+YKuGk8Opf3n5r4D8P+pMW5FonS+BFQY4yA1HUqQH7tBygKESDR+8Vd/6NvvvgwIH554SqTi3P/7zf9Z8Gl4iWDm/A5ziUohM4S8jMX98TPEqABAUgCKpAHykAd6ABDYAasgC1wBG7AG/iDEBAJVgMWSASpgA+yQB7YBApBMdgJ9oBqUAcaQTNoBcdBJzgFzoNL4Bq4AW6D+2AUTIBnYBa8BgsQBGEhMkSB5CEVSBPSh8wgBmQPuUG+UBAUCcVCCRAPEkJ50GaoGCqDqqF6qBn6HjoJnYeuQIPQXWgMmoZ+h97BCEyCqbASrAUbwwzYCfaBQ+BVcAK8Bs6FC+AdcCXcAB+FO+Dz8DX4NjwKP4PnEIAQERqiihgiDMQF8UeikHiEj6xHipAKpAFpRbqRPuQmMorMIG9RGBQFRUcZomxRnqhQFAu1BrUeVYKqRh1GdaB6UTdRY6hZ1Ec0Ga2I1kfboL3QEegEdBa6EF2BbkK3oy+ib6Mn0K8xGAwNo42xwnhiIjFJmLWYEsw+TBvmHGYQM46Zw2Kx8lh9rB3WH8vECrCF2CrsUexZ7BB2AvsGR8Sp4Mxw7rgoHA+Xj6vAHcGdwQ3hJnELeCm8Jt4G749n43PwpfhGfDf+On4Cv0CQJmgT7AghhCTCJkIloZVwkfCA8JJIJKoRrYmBRC5xI7GSeIx4mThGfEuSIemRXEjRJCFpB+kQ6RzpLuklmUzWIjuSo8gC8g5yM/kC+RH5jQRFwkjCS4ItsUGiRqJDYkjiuSReUlPSSXK1ZK5kheQJyeuSM1J4KS0pFymm1HqpGqmTUiNSc9IUaVNpf+lU6RLpI9JXpKdksDJaMm4ybJkCmYMyF2TGKQhFneJCYVE2UxopFykTVAxVm+pFTaIWU7+jDlBnZWVkl8mGyWbL1sielh2lITQtmhcthVZKO04bpr1borTEaQlnyfYlrUuGlszLLZVzlOPIFcm1yd2WeydPl3eTT5bfJd8p/1ABpaCnEKiQpbBf4aLCzFLqUtulrKVFS48vvacIK+opBimuVTyo2K84p6Ss5KGUrlSldEFpRpmm7KicpFyufEZ5WoWiYq/CVSlXOavylC5Ld6Kn0CvpvfRZVUVVT1Whar3qgOqCmrZaqFq+WpvaQ3WCOkM9Xr1cvUd9VkNFw08jT6NF454mXpOhmai5V7NPc15LWytca6tWp9aUtpy2l3audov2Ax2yjoPOGp0GnVu6GF2GbrLuPt0berCehV6iXo3edX1Y31Kfq79Pf9AAbWBtwDNoMBgxJBk6GWYathiOGdGMfI3yjTqNnhtrGEcZ7zLuM/5oYmGSYtJoct9UxtTbNN+02/R3Mz0zllmN2S1zsrm7+QbzLvMXy/SXcZbtX3bHgmLhZ7HVosfig6WVJd+y1XLaSsMq1qrWaoRBZQQwShiXrdHWztYbrE9Zv7WxtBHYHLf5zdbQNtn2iO3Ucu3lnOWNy8ft1OyYdvV2o/Z0+1j7A/ajDqoOTIcGh8eO6o5sxybHSSddpySno07PnU2c+c7tzvMuNi7rXM65Iq4erkWuA24ybqFu1W6P3NXcE9xb3Gc9LDzWepzzRHv6eO7yHPFS8mJ5NXvNelt5r/Pu9SH5BPtU+zz21fPl+3b7wX7efrv9HqzQXMFb0ekP/L38d/s/DNAOWBPwYyAmMCCwJvBJkGlQXlBfMCU4JvhI8OsQ55DSkPuhOqHC0J4wybDosOaw+XDX8LLw0QjjiHUR1yIVIrmRXVHYqLCopqi5lW4r96yciLaILoweXqW9KnvVldUKq1NWn46RjGHGnIhFx4bHHol9z/RnNjDn4rziauNmWS6svaxnbEd2OXuaY8cp40zG28WXxU8l2CXsTphOdEisSJzhunCruS+SPJPqkuaT/ZMPJX9KCU9pS8Wlxqae5Mnwknm9acpp2WmD6frphemja2zW7Fkzy/fhN2VAGasyugRU0c9Uv1BHuEU4lmmfWZP5Jiss60S2dDYvuz9HL2d7zmSue+63a1FrWWt78lTzNuWNrXNaV78eWh+3vmeD+oaCDRMbPTYe3kTYlLzpp3yT/LL8V5vDN3cXKBVsLBjf4rGlpVCikF84stV2a9021DbutoHt5turtn8sYhddLTYprih+X8IqufqN6TeV33zaEb9joNSydP9OzE7ezuFdDrsOl0mX5ZaN7/bb3VFOLy8qf7UnZs+VimUVdXsJe4V7Ryt9K7uqNKp2Vr2vTqy+XeNc01arWLu9dn4fe9/Qfsf9rXVKdcV17w5wD9yp96jvaNBqqDiIOZh58EljWGPft4xvm5sUmoqbPhziHRo9HHS4t9mqufmI4pHSFrhF2DJ9NProje9cv+tqNWytb6O1FR8Dx4THnn4f+/3wcZ/jPScYJ1p/0Pyhtp3SXtQBdeR0zHYmdo52RXYNnvQ+2dNt293+o9GPh06pnqo5LXu69AzhTMGZT2dzz86dSz83cz7h/HhPTM/9CxEXbvUG9g5c9Ll4+ZL7pQt9Tn1nL9tdPnXF5srJq4yrndcsr3X0W/S3/2TxU/uA5UDHdavrXTesb3QPLh88M+QwdP6m681Lt7xuXbu94vbgcOjwnZHokdE77DtTd1PuvriXeW/h/sYH6AdFD6UeVjxSfNTws+7PbaOWo6fHXMf6Hwc/vj/OGn/2S8Yv7ycKnpCfVEyqTDZPmU2dmnafvvF05dOJZ+nPFmYKf5X+tfa5zvMffnP8rX82YnbiBf/Fp99LXsq/PPRq2aueuYC5R69TXy/MF72Rf3P4LeNt37vwd5MLWe+x7ys/6H7o/ujz8cGn1E+f/gUDmPP8usTo0wAAAwBQTFRF////4ubvAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA6UZphQAAAAlwSFlzAAALEwAACxMBAJqcGAAAAAd0SU1FB94HGBARBmXxUcoAAADPSURBVHja7dehDcAwEARBp/+mU4BRQKQ9aQwfrQ55zjnP9Vq3emBtr8XAduJGYDlxJbCbuBNYTVwKbCZuBRYT1wJ7iXuB/tNfA5mESZiESZiESZiESZiESZiESZiESZiESZiESZiESZiESZiESZjEhkzCJEzCJEzCJEzCJEzCJEzCJEzCJEzCJEzCJEzCJEzCJEzCJDZkEiZhEiZhEiZhEiZhEiZhEiZhEiZhEiZhEiZhEiZhEiZhEiaxIZMwCZMwCZMwCZMwCZMwCZP8ensBjf9aAS7J1I8AAAAASUVORK5CYII=);";
        private const string CssStyle = 
            @"(function(){var css = 'html { " + ImageCss + @" } body > img:first-child { width: 100%; height:auto; }',
                    head = document.head || document.getElementsByTagName('head')[0],
                    style = document.createElement('style');

                if (document.body.children.length > 1) {
                    css = 'video { position: relative !important; z-index: 9999999; }';
                }

                style.type = 'text/css';
                if (style.styleSheet){
                  style.styleSheet.cssText = css;
                } else {
                  style.appendChild(document.createTextNode(css));
                }

                head.appendChild(style);
              }());";

        /// <summary>
        /// The view model.
        /// </summary>
        private ViewLinksViewModel viewModel;

        /// <summary>
        /// The data transfer manager.
        /// </summary>
        private DataTransferManager dataTransferManager;

        public View()
            : this(new NewSlapsStore(), new AccountStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        /// <param name="newSlapsStore">
        /// The new Slaps Store.
        /// </param>
        /// <param name="accountStore">
        /// The account Store.
        /// </param>
        public View(INewSlapsStore newSlapsStore, AccountStore accountStore)
        {
            this.newSlapsStore = newSlapsStore;
            this.accountStore = accountStore;
            this.InitializeComponent();

            this.viewModel = this.DataContext as ViewLinksViewModel;
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += this.ShareStream;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="eventArgs">
        /// The event Args.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            var links = eventArgs.Parameter as ViewLinksViewModel;

            if (links == null)
            {
                // TODO - Go back and show error message
                return;
            }

            this.viewModel.Links = links.Links;
            this.viewModel.SelectedItem = links.SelectedItem;

            base.OnNavigatedTo(eventArgs);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.dataTransferManager.DataRequested -= this.ShareStream;
            base.OnNavigatedFrom(e);
        }

        private async void ViewLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            //if (!this.IsImageUrl(this.viewModel.SelectedItem.Url))
            //{
            //    return;
            //}

            await sender.InvokeScriptAsync("eval", new[] { CssStyle });
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        private bool IsImageUrl(string URL)
        {
            var url = URL.ToUpper();

            return ImageExtensions.Any(url.Contains);
        }

        /// <summary>
        /// The flip view changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FlipViewChanged(object sender, SelectionChangedEventArgs e)
        {
            var link = this.viewModel.SelectedItem;
            ToastNotificationManager.History.Remove(link.Id.ToString());
            // this.newSlapsStore.RemoveLink(link.Id);
        }

        /// <summary>
        /// The rectangle tapped.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RectangleTapped(object sender, TappedRoutedEventArgs e)
        {
            this.viewModel.UiVisibility = Visibility.Collapsed;

            this.NavigationHelper.Disabled = true;

            HardwareButtons.BackPressed += this.HardwareButtonsBackPressed;
            
        }

        /// <summary>
        /// The hardware buttons_ back pressed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void HardwareButtonsBackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            HardwareButtons.BackPressed -= this.HardwareButtonsBackPressed;
            this.viewModel.UiVisibility = Visibility.Visible;
            this.NavigationHelper.Disabled = false;
        }

        /// <summary>
        /// The reply click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReplyClick(object sender, RoutedEventArgs e)
        {
            this.Navigate<FindGifs>(this.viewModel.SelectedItem.StreamKey);
        }

        /// <summary>
        /// The share click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ShareClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        /// <summary>
        /// The share stream.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void ShareStream(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var data = args.Request.Data;

            var streamUrl = string.Format("https://linkslap.me/s/{0}", this.viewModel.SelectedItem.StreamKey);

            data.Properties.Title = string.Format(
                "{0} shared from {1} on Linkslap - {2}",
                this.viewModel.SelectedItem.Comment,
                this.viewModel.SelectedItem.StreamName,
                streamUrl);

            data.Properties.Description = string.Format("Instantly share pics or pages with your friends!");
            data.SetUri(this.viewModel.SelectedItem.Uri);
        }

        /// <summary>
        /// The go home.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GoHome(object sender, RoutedEventArgs e)
        {
            this.NavigateRoot<Home>();
        }

        /// <summary>
        /// The reload browser.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReloadBrowser(object sender, RoutedEventArgs e)
        {
            this.viewModel.SelectedItem.Uri = new Uri(this.viewModel.SelectedItem.Url, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// The open in ie.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OpenInIe(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(this.viewModel.SelectedItem.Uri);
        }

        /// <summary>
        /// The logout click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            ToastNotificationManager.History.Clear();
            this.accountStore.Logout();

            this.NavigateRoot<Login>();
        }

    }
}
