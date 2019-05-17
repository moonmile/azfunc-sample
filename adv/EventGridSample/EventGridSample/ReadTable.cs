using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace EventGridSample
{

    public static class ReadTable
    {
        /// <summary>
        /// テーブルストレージのバインドサンプル
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cloudTable"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("ReadTable")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("events")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("called ReadTable");
            // データ読み込み
            var query = new TableQuery<EventEntity>();
            var items = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            foreach (var it in items)
            {
                log.LogInformation($"{it.RowKey} {it.Url} {it.Funcname}");
            }
            var lst = new List<EventEntity>();
            foreach ( var it in items )
            {
                lst.Add(it);
            }
            string json = JsonConvert.SerializeObject(lst);
            return new OkObjectResult( json );
        }

        /// <summary>
        /// テーブルストレージへのバインド
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("WriteTable")]
        [return: Table("events")]
        public static EventEntity Run2(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called WriteTable");
            // データ挿入
            var item = new EventEntity()
            {
                Url = "http://localhost/" + DateTime.Now.ToString(),
                Funcname = "in WriteTable",
                PartitionKey = "Japan",
                RowKey = Guid.NewGuid().ToString(),
            };
            return item;
        }
    }
}
