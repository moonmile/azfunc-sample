using Microsoft.Azure.Documents.Client;
using System;

namespace ConsoleCosmosDB
{
    class Program
    {
        static readonly string Endpoint = "https://localhost:8081";
        static readonly string Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        static readonly string DatabaseId = "Alerts";
        static readonly string CollectionId = "Messages";

        static void Main(string[] args)
        {
            var app = new Program();
            app.Go(args);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        async void Go( string[] args )
        {
            Console.WriteLine("Send alart message");
            var client = new DocumentClient(new Uri(Endpoint), Key);
            var msg = new Message()
            {
                region = "Japan",
                name = args[0],
                message = args[1],
                priority = args.Length < 3 ? null : args[2]
            };
            await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), msg);
        }
        public class Message
        {
            public string region { get; set; }
            public string name { get; set; }
            public string message { get; set; }
            public string priority { get; set; }
        }
    }
}
