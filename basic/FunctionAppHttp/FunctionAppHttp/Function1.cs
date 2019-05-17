using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppHttp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }


        [FunctionName("Function2")]
        public static IActionResult Run2(
            [HttpTrigger(AuthorizationLevel.Function, "get", 
            Route = "products/{category:alpha}/{id:int?}")]
                HttpRequest req,
                string category,
                int? id,
                ILogger log)
        {
            if ( id == null )
            {
                return new OkObjectResult($"category {category} and all items.");
            }
            else
            {
                return new OkObjectResult($"category {category} and id = {id}.");
            }
        }
    }
}
