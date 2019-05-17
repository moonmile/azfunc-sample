using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HttpSample.Models;
using System.Collections.Generic;
using System.Linq;


namespace HttpSample
{
    public static class ReadData
    {
        [FunctionName("ReadData")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called ReadData");

            var context = new azuredbContext();
            var items = context.Persons.ToList();
            /// åüçıÇµÇΩåãâ ÇJSONå`éÆÇ≈ï‘Ç∑
            var res = JsonConvert.SerializeObject(items);
            return new OkObjectResult(res);
        }
    }
}
