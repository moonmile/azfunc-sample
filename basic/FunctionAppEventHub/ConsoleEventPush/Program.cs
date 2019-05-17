using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEventPush
{
    class Program
    {
        private const string EventHubConnectionString = "";
        private const string EventHubName = "samples-workitems";

        public static void Main(string[] args)
        {
            var app = new Program();
            app.Go();
            Console.WriteLine("Any hit key.");
            Console.ReadLine();
        }

        private async void Go()
        {
            // Eevnt Hub への接続
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            // 1秒間隔で10回メッセージを送信する
            for ( int i=1; i<=10; i++ )
            {
                var message = $"Message {i} ";
                Console.WriteLine($"Sending message: {message}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                await Task.Delay(1000);
            }
            // クローズ処理
            await eventHubClient.CloseAsync();
        }
    }
}
