using Microsoft.Azure.NotificationHubs;
using System;

namespace ConsoleAppPush
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Azure Notification Hub World!");
            var app = new Program();
            app.Go();
            Console.WriteLine("any hit key.");
            Console.ReadKey();
        }

        void Go()
        {
            var message = "sample message at " + DateTime.Now.ToString();
            NotificationHubClient hub =
                NotificationHubClient.CreateClientFromConnectionString(
                    "",
                    "sample-azfunc-notification");
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                    message + "</text></binding></visual></toast>";
#if false
              Microsoft.Azure.NotificationHubs.NotificationOutcome outcome =
                hub.SendWindowsNativeNotificationAsync(toast).Result;
#else

            var deviceid = "";
            var win = new WindowsNotification(toast);
            var outcome = hub.SendDirectNotificationAsync(win, deviceid).Result;
#endif
            Console.WriteLine("Status: " + outcome.State.ToString());
        }
    }
}
