namespace Linkslap.WP.Communication.Interfaces
{
    using System.Collections.ObjectModel;

    using Linkslap.WP.Communication.Models;

    public interface ISettingsStore
    {

        bool DisableAllNotifications { get; set; }

        ObservableCollection<SubscriptionSettings> SubscriptionSettings { get; }

        bool ShowPushNotification(string streamKey);

        bool ShowInNewLinks(string streamKey);

        void SaveSubscriptionSettings();

        void RemoveSubscriptionSetting(int subscriptionId);
    }
}
