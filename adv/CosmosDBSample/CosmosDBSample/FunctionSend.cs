using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CosmosDBSample
{
    public static class FunctionSend
    {
        [FunctionName("FunctionSend")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, 
            "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation($"called FunctionSend {DateTime.Now}");
            string requestBody = 
                await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;
            var message = data?.message;

            log.LogInformation($"from {name} : {message}");
            return (ActionResult)new OkObjectResult("ok");
        }
    }
}
