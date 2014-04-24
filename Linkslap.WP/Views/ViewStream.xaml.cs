using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Linkslap.WP.Views
{
    using System.Collections.ObjectModel;

    using Windows.UI.Core;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.Controls;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewStream : PageBase
    {
        private readonly IStreamStore streamStore;

        public ViewStream()
            : this(new StreamStore())
        {
        }

        public ViewStream(IStreamStore streamStore)
        {
            this.streamStore = streamStore;

            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="eventArgs">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            var subscription = eventArgs.Parameter as SubscriptionViewModel;

            if (subscription == null)
            {
                return;
            }

            var collection = new ObservableCollection<LinkViewModel>();
            collection.AddRange(subscription.Links);

            this.DataContext = collection;

            var task = this.streamStore.GetStreamLinks(subscription.StreamKey);

            task.ContinueWith(
                links => this.Run(
                    () =>
                        {
                            var result = Mapper.Map(links.Result, new List<LinkViewModel>());
                            collection.AddRange(result);
                        }));


            base.OnNavigatedTo(eventArgs);
        }
    }
}
