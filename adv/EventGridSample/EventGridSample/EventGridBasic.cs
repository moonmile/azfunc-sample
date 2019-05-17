// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Azure.EventGrid;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace EventGridSample
{
    /// Event Grid トリガーを使った場合は、自動認証される。
    public static class EventGridTriggerBasic
    {
        /// <summary>
        /// ログ出力するだけのサンプルコード、ひな形のまま
        /// </summary>
        /// <param name="eventGridEvent"></param>
        /// <param name="log"></param>
        [FunctionName("EventGridTriggerBasic")]
        public async static void Run([EventGridTrigger]
            EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent.EventType == "Microsoft.Storage.BlobCreated")
            {
                var obj = eventGridEvent.Data as JObject;
                var blob = obj.ToObject<StorageBlobCreatedEventData>();
                log.LogInformation("topic: " + eventGridEvent.Topic);
                log.LogInformation("subject: " + eventGridEvent.Subject);
                log.LogInformation("url: " + blob.Url);
            }
        }
    }
}
