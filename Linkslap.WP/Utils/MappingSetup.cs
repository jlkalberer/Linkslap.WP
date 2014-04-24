namespace Linkslap.WP.Utils
{
    using AutoMapper;

    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// The mapping setup.
    /// </summary>
    public static class MappingSetup
    {
        /// <summary>
        /// The map.
        /// </summary>
        public static void Map()
        {
            Subscriptions();
        }

        /// <summary>
        /// The subscriptions.
        /// </summary>
        private static void Subscriptions()
        {
            Mapper.CreateMap<Subscription, SubscriptionViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.StreamKey, m => m.MapFrom(s => s.Stream.Key))
                .ForMember(d => d.Name, m => m.MapFrom(s => s.Stream.Name))
                .ForMember(d => d.Links, m => m.Ignore());

            Mapper.CreateMap<Link, LinkViewModel>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
