using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Linkslap.WP.Controls
{
    using System.Collections;
    using System.Collections.ObjectModel;

    using Linkslap.WP.ViewModels;

    /// <summary>
    /// The link long list.
    /// </summary>
    public partial class LinkLongList : UserControl
    {
        /// <summary>
        /// The links property.
        /// </summary>
        public static readonly DependencyProperty LinksProperty = DependencyProperty.Register(
            "Links",
            typeof(ObservableCollection<LinkViewModel>),
            typeof(LinkLongList),
            new PropertyMetadata(OnLinkItemsChanged));

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
            (d as LinkLongList).LongListSelector.ItemsSource = e.NewValue as IList;
        }
    }
}
