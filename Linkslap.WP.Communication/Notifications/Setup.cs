namespace Linkslap.WP.Communication
{
    using System;
    using System.Runtime.InteropServices.ComTypes;

    using Linkslap.WP.Communication.Util;
    using Windows.Networking.PushNotifications;
    using Microsoft.WindowsAzure.Messaging;
    using Windows.Storage;

    public class Setup
    {
        public async void Run()
        {
            var hub = new NotificationHub("<hub name>", "<connection string with listen access>");
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            await hub.RegisterNativeAsync(channel.Uri);

            //var channel = HttpNotificationChannel.Find("Linkslap");
            //if (channel == null)
            //{
            //    channel = new HttpNotificationChannel("Linkslap");
            //    channel.Open();
            //    channel.BindToShellToast();
            //}

            //channel.ChannelUriUpdated += async (o, args) =>
            //{
            //    var hub = new NotificationHub(AppSettings.NotificationHubPath, AppSettings.HubConnectionString);
            //    await hub.RegisterNativeAsync(args.ChannelUri.ToString());
            //};

            //channel.HttpNotificationReceived += (o, args) =>
            //{
            //    var v = 0;
            //};

            //channel.ShellToastNotificationReceived += (o, args) =>
            //{
            //    var v = 0;
            //};

            //channel.ConnectionStatusChanged += (o, args) =>
            //{
            //    var v = 0;
            //};

            //channel.ErrorOccurred += (o, args) =>
            //{
            //    var v = 0;
            //};
        }
    }
}
