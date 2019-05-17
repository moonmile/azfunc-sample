using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDevice
{
    class Program
    {
        private static void Main(string[] args)
        {
            var app = new Program();
            app.Go();
            Console.WriteLine("Any hit key.");
            Console.ReadKey();
        }
        private readonly static string connectionString = "";
        private async void Go()
        {
            // IoT Hub に MQTT で接続
            var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);

            // ランダムで送信する
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                // Create JSON message
                var telemetryDataPoint = new
                {
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");
                // メッセージを送信する
                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                // 1秒間待つ
                await Task.Delay(1000);
            }
        }
    }
}
