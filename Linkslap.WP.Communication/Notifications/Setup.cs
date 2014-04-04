namespace Linkslap.WP.Communication
{
    using Linkslap.WP.Communication.Util;

    using Microsoft.Phone.Notification;
    using Microsoft.WindowsAzure.Messaging;

    public class Setup
    {
        public void Run()
        {
            var channel = HttpNotificationChannel.Find("Linkslap");
            if (channel == null)
            {
                channel = new HttpNotificationChannel("Linkslap");
                channel.Open();
                channel.BindToShellToast();
            }

            channel.ChannelUriUpdated += async (o, args) =>
            {
                var hub = new NotificationHub(AppSettings.NotificationHubPath, AppSettings.HubConnectionString);
                await hub.RegisterNativeAsync(args.ChannelUri.ToString());
            };

            channel.HttpNotificationReceived += (o, args) =>
            {
                var v = 0;
            };

            channel.ShellToastNotificationReceived += (o, args) =>
            {
                var v = 0;
            };

            channel.ConnectionStatusChanged += (o, args) =>
            {
                var v = 0;
            };

            channel.ErrorOccurred += (o, args) =>
            {
                var v = 0;
            };
        }
    }
}
