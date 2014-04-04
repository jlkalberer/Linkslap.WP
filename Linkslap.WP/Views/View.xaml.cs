using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Linkslap.WP
{
    public partial class View : PhoneApplicationPage
    {
        public View()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var query = NavigationContext.QueryString;
            if (query.ContainsKey("url"))
            {
                this.browser.Navigate(new Uri(query["url"], UriKind.Absolute));
            }
            base.OnNavigatedTo(e);
        }

    }
}