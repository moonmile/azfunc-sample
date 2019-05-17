using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventGridSample
{
    public static class HttpTriggerManual
    {
        [FunctionName("HttpTriggerManual")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called HttpTriggerManual");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string validationUrl = data[0]?.data.validationUrl;
            log.LogInformation("validationUrl : " + validationUrl);
            return new OkObjectResult("ok");
        }
    }
}
