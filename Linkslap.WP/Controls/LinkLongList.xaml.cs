// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Linkslap.WP.Controls
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Linkslap.WP.ViewModels;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using Linkslap.WP.Views;

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
        /// Initializes a new instance of the <see cref="LinkLongList"/> class.
        /// </summary>
        public LinkLongList()
        {
            this.InitializeComponent();
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
    }
}
