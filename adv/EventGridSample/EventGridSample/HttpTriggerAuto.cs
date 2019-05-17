using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;


using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventGrid;

namespace EventGridSample
{
    public static class HttpTriggerAuto
    {
        /// Event Grid からの検証を、自分の webhook で行う場合は
        /// コードから validationCode を抜き出し、応答を返す
        [FunctionName("HttpTriggerAuto")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called HttpTriggerAuto");
            var validationEventType = "Microsoft.EventGrid.SubscriptionValidationEvent";
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var eventGridSubscriber = new EventGridSubscriber();
            var events = eventGridSubscriber.DeserializeEventGridEvents(requestBody);
            var data = events[0];
            if (data.EventType == validationEventType)
            {
                // 検証コードを自動で返す
                var eventData = data.Data as SubscriptionValidationEventData;
                var responseData = new SubscriptionValidationResponse()
                {
                    ValidationResponse = eventData.ValidationCode
                };
                return new OkObjectResult(responseData);
            }
            // ここは実際の処理
            log.LogInformation("your code in HttpTriggerAuto.");
            // ポストされたデータを表示
            log.LogInformation(requestBody);
            return new OkObjectResult("ok");
        }
    }
}
