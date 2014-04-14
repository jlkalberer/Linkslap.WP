namespace Linkslap.WP.Views
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Navigation;

    using AutoMapper;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;
    using Linkslap.WP.ViewModels;

    using Microsoft.Phone.Controls;

    public partial class ViewStream : PhoneApplicationPage
    {
        private readonly ISubscriptionRepository subscription;

        private readonly IStreamRepository streamRepository;

        private SubscriptionViewModel viewModel;

        public ViewStream()
            : this(new SubscriptionRepository(), new StreamRepository())
        {
        }

        public ViewStream(ISubscriptionRepository subscription, IStreamRepository streamRepository)
        {
            this.subscription = subscription;
            this.streamRepository = streamRepository;
            
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string subscriptionId;
            if (NavigationContext.QueryString.TryGetValue("subScriptionId", out subscriptionId))
            {
                var id = int.Parse(subscriptionId);

                this.viewModel = Mapper.Map<Subscription, SubscriptionViewModel>(this.subscription.GetSubscription(id));
                this.DataContext = this.viewModel;

                var task = this.streamRepository.GetStreamLinks(this.viewModel.StreamKey);

                task.ContinueWith(
                    links =>
                        {
                            var result = Mapper.Map(links.Result, this.viewModel.Links);
                            this.viewModel.Links.AddRange(result);
                        });
            }

            base.OnNavigatedTo(e);
        }
    }
}