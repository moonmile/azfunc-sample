using System;
using System.Net.Http;

namespace ConsoleSlack
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Program();
            app.Go(args);
        }
        void Go(string[] args)
        {
            Console.WriteLine("Send Slack message");
            var username = args[0];
            var text = args[1];
            var channel = args.Length == 3 ? args[2] : "";
            var URL = "";

            var cl = new HttpClient();
            var json = $"{{ \"username\": \"{username}\", \"text\": \"{text}\", \"channel\": \"{channel}\" }}";
            Console.WriteLine(json);
            var content = new StringContent(json);
            var res = cl.PostAsync(URL, content).Result;
        }
    }
}
