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
using Microsoft.EntityFrameworkCore;

namespace HttpSample
{
    public static class WriteData
    {
        [FunctionName("WriteData")]                                                 // �@
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]		// �A
	        HttpRequest req, ILogger log)
        {
            log.LogInformation("called WriteData");                                 // �B
            // POSTデータからパラメーターを取得
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync(); // �C
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string pno = data?.pno;
            string status = data?.status;
            // パラメータのチェック
            if (pno == null || status == null)                                      // �D
            {
                return new BadRequestObjectResult(
                    "ERROR: 社員番号(pno)と状態(status)を指定して下さい");
            }
            // データを更新
            var context = new azuredbContext();                                     // �E
            var item = await context.Persons.FirstOrDefaultAsync(t => t.PersonNo == pno);
            if (item == null)                                                       // �F
            {
                return new BadRequestObjectResult(
                    "ERROR: 社員番号(pno)が見つかりません");
            }
            // 出勤状態を更新
            item.Status = status;                                                   // �G
            item.ModifiedAt = DateTime.Now;
            context.Persons.Update(item);
            await context.SaveChangesAsync();
            // 更新結果を返す
            return new OkObjectResult(                                              // �H
                "SUCCESS: 更新しました " + DateTime.Now.ToString());
        }
    }
}
