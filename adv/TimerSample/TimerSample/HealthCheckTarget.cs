using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TimerSample
{
    public static class HealthCheckTarget
    {
        /// <summary>
        /// ヘルスチェック用のターゲット関数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("HealthCheckTarget")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            /// ログ出力
            log.LogInformation($"HealthCheckTarget called: {DateTime.Now}");
            /// パラメータを取得
            string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            /// ヘルスチェックの戻り値
            return name != null
                ? (ActionResult)new OkObjectResult("OK")
                : new BadRequestObjectResult("NG");
        }
    }
}
