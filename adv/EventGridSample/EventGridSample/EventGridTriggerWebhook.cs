// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

using System.Net.Http;

namespace EventGridSample
{
    public static class EventGridTriggerWebhook
    {
        static HttpClient cl = new HttpClient();
        static string URL = System.Environment.GetEnvironmentVariable("OTHER_WEBAPI_URL");
        /// <summary>
        /// 外部のwebhookを呼び出すサンプル
        /// </summary>
        /// <param name="eventGridEvent"></param>
        /// <param name="log"></param>
        [FunctionName("EventGridTriggerWebhook")]
        public async static void Run([EventGridTrigger]
            EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent.EventType == "Microsoft.Storage.BlobCreated")
            {
                dynamic blob = eventGridEvent.Data;
                var funcname = "EventGridTriggerWebhook";
                string subject = eventGridEvent.Subject;
                string url = blob?.url;
            var content = new StringContent(
                $"{{ \"funcname\": \"{funcname}\", " + 
                $" \"url\": \"{url}\", " + 
                $"\"subject\": \"{subject}\" }}");
                var res = await cl.PostAsync(URL, content);
            }
        }
    }
}
