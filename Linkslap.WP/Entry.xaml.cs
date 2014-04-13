using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Linkslap.WP.Resources;

namespace Linkslap.WP
{
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Utils;

    public partial class Entry : PhoneApplicationPage
    {
        private readonly IAccountRepository accountRepository;

        public Entry()
            : this(new AccountRepository())
        {
        }

        public Entry(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
            this.InitializeComponent();

            var task = this.accountRepository.Get();
            task.ContinueWith(
                t =>
                {
                    if (t.Result == null)
                    {
                        this.Navigate("/Views/Login.xaml");
                        return;
                    }

                    this.Navigate("/Views/Home.xaml");
                });
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}