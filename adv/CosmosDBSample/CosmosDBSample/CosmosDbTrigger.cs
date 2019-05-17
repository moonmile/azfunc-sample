using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CosmosDBSample
{
    public static class CosmosDbTrigger
    {
        static HttpClient cl = new HttpClient();

        [FunctionName("FuncWebApi")]
        public static async void Run([CosmosDBTrigger(
            databaseName: "Alerts",
            collectionName: "Messages",
            ConnectionStringSetting = "COSMOSDB_CONNECTION",
            LeaseCollectionName = "leases",
            LeaseCollectionPrefix = "normal",
            CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                var doc = input[0];
                var name = doc.GetPropertyValue<string>("name");
                var message = doc.GetPropertyValue<string>("message");
                var data = new SendHttp()
                {
                    name = name,
                    message = message
                };
                log.LogInformation("called webapi");
                await cl.PostAsJsonAsync("http://localhost:7071/api/FunctionSend", data);
            }
        }
        public class SendHttp
        {
            public string name { get; set; }
            public string message { get; set; }
        }

        [FunctionName("FuncSlack")]
        public static async void RunSlack([CosmosDBTrigger(
            databaseName: "Alerts",
            collectionName: "Messages",
            ConnectionStringSetting = "COSMOSDB_CONNECTION",
            LeaseCollectionName = "leases",
            LeaseCollectionPrefix = "priority",
            CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                var doc = input[0];
                var priority = doc.GetPropertyValue<string>("priority");
                var name = doc.GetPropertyValue<string>("name");
                var message = doc.GetPropertyValue<string>("message");
                /// 優先度が設定されている場合はSlackに通知する
                // Slackに通知する
                if (!string.IsNullOrEmpty(priority))
                {
                    var data = new Slack()
                    {
                        name = name,
                        text = message,
                        channel = "#samples"
                    };
                    log.LogInformation("called slack");
                    await cl.PostAsJsonAsync("", data);
                }
            }
        }
        public class Slack
        {
            public string name { get; set; }
            public string text { get; set; }
            public string channel { get; set; }

        }
    }
}
