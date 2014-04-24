// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Linkslap.WP.Controls
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
            typeof(ObservableCollection<LinkViewModel>),
            typeof(LinkLongList),
            new PropertyMetadata(null, OnLinkItemsChanged));

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
        public ObservableCollection<LinkViewModel> Links
        {
            get { return (ObservableCollection<LinkViewModel>)GetValue(LinksProperty); }
            set
            {
                this.SetValue(LinksProperty, value);
            }
        }

        private static void OnLinkItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LinkLongList).DataContext = e.NewValue;
        }
    }
}
