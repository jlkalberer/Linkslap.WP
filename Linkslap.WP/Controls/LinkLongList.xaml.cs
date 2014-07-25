// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
namespace Linkslap.WP.Controls
{
    using System.Collections.Generic;

    using Linkslap.WP.ViewModels;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class LinkLongList : UserControl
    {
        /// <summary>
        /// The links property.
        /// </summary>
        public static readonly DependencyProperty LinksProperty = DependencyProperty.Register(
            "Links",
            typeof(IEnumerable<LinkViewModel>),
            typeof(LinkLongList),
            new PropertyMetadata(default(IEnumerable<LinkViewModel>), OnLinkItemsChanged));

        /// <summary>
        /// The links property.
        /// </summary>
        public static readonly DependencyProperty RemovableLinksProperty = DependencyProperty.Register(
            "RemovableLinks",
            typeof(Visibility),
            typeof(LinkLongList),
            new PropertyMetadata(Visibility.Collapsed, OnRemovableLinksChanged));


        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLongList"/> class.
        /// </summary>
        public LinkLongList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether removable links.
        /// </summary>
        public Visibility RemovableLinks
        {
            get
            {
                return (Visibility)GetValue(RemovableLinksProperty);
            }

            set
            {
                this.SetValue(RemovableLinksProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        public IEnumerable<LinkViewModel> Links
        {
            get
            {
                return (IEnumerable<LinkViewModel>)GetValue(LinksProperty);
            }

            set
            {
                this.SetValue(LinksProperty, value);
            }
        }

        /// <summary>
        /// The selection changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// The remove item clicked.
        /// </summary>
        public event RoutedEventHandler RemoveItemClicked;

        /// <summary>
        /// The on link items changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnLinkItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LinkLongList).ListView.ItemsSource = e.NewValue;
        }

        private static void OnRemovableLinksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var v = 0;
        }

        /// <summary>
        /// The selector_ on selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChanged == null)
            {
                return;
            }

            this.SelectionChanged(sender, e);
        }

        /// <summary>
        /// The remove click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            if (this.RemoveItemClicked != null)
            {
                this.RemoveItemClicked(sender, e);
            }
        }
    }
}
