// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

using Microsoft.WindowsAzure.Storage.Table;

namespace EventGridSample
{
    public static class EventGridTriggerTable
    {
        /// <summary>
        /// テーブルストレージに書き込むサンプル
        /// </summary>
        /// <param name="eventGridEvent"></param>
        /// <param name="log"></param>
        [FunctionName("EventGridTriggerTable")]
        [return: Table("events", Connection = "STORAGE_CONNECTION")]
        public static EventEntity Run([EventGridTrigger]
            EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent.EventType == "Microsoft.Storage.BlobCreated")
            {
                dynamic blob = eventGridEvent.Data;
                string subject = eventGridEvent.Subject;
                string url = blob?.url;

                // データ挿入
                var item = new EventEntity()
                {
                    Funcname = "EventGridTriggerTable",
                    Url = url,
                    Subject = subject,
                    PartitionKey = "Japan",
                    RowKey = System.Guid.NewGuid().ToString(),
                };
                return item;
            }
            return null;
        }
    }
    public class EventEntity : TableEntity
    {
        public string Funcname { get; set; }
        public string Url { get; set; }
        public string Subject { get; set; }
    }
}
