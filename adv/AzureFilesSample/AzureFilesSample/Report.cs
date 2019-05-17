using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using ClosedXML.Excel;
using System.Linq;
using AzureFilesSample.Models;

namespace AzureFilesSample
{
    public static class Report
    {


        /// <summary>
        /// レポート作成用の関数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("MakeReport")]
        public static async Task<IActionResult> MakeReport(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called MakeReport");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                System.Environment.GetEnvironmentVariable("STORAGE_CONNECTION"));
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference("shared");
            CloudFileDirectory root = share.GetRootDirectoryReference();
            var download = root.GetDirectoryReference("download");
            var tempfile = download.GetFileReference("template.xlsx");
            var filename = "output-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var file = download.GetFileReference(filename);
            using (var st = await tempfile.OpenReadAsync())
            {
                var wb = new XLWorkbook(st);
                var sh = wb.Worksheets.First();

                var context = new azuredbContext();
                var items = context.AddressBook.ToList();
                int row = 2;
                foreach (var it in items)
                {
                    sh.Cell(row, 1).Value = it.ID;
                    sh.Cell(row, 2).Value = it.Company;
                    sh.Cell(row, 3).Value = it.Person;
                    sh.Cell(row, 4).Value = it.Apartment;
                    row++;
                }
                var mem = new System.IO.MemoryStream();
                wb.SaveAs(mem);
                mem.Position = 0;
                await file.UploadFromStreamAsync(mem);
            }
            return new OkObjectResult($"excel download/{filename} success");
        }

        /// <summary>
        /// レポート読み込み用の関数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("ReadReport")]
        public static async Task<IActionResult> ReadReport(
            [HttpTrigger(AuthorizationLevel.Function,
            "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called ReadReport");
            /// upload フォルダーから、帳票ファイルを探す
            var cnstr = System.Environment.GetEnvironmentVariable("STORAGE_CONNECTION");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cnstr);
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference("shared");
            CloudFileDirectory root = share.GetRootDirectoryReference();
            var upload = root.GetDirectoryReference("upload");
            var file = upload.GetFileReference("sample.xlsx");
            var result = "NG";
            if (await file.ExistsAsync())
            {
                using (var st = await file.OpenReadAsync())
                {
                    var wb = new XLWorkbook(st);
                    var sh = wb.Worksheets.First();
                    var id = sh.Cell(1, 2).GetValue<int>();       // ID
                    var company = sh.Cell(2, 2).GetString();      // 会社
                    var person = sh.Cell(3, 2).GetString();       // 担当者
                    var apartment = sh.Cell(4, 2).GetString();    // 部署
                    log.LogInformation($"{id} {company} {person} {apartment}");
                    result = $"{id} {company} {person} {apartment}";
                    // データベースへの書き込み
                    var context = new azuredbContext();
                    var item = new AddressBook()
                    {
                        Company = company,
                        Person = person,
                        Apartment = apartment
                    };
                    context.Add(item);
                    context.SaveChanges();
                }
            }
            return new OkObjectResult(result);
        }
    }
#if false
    public static class Report_
{
    [FunctionName("MakeReport")]
    public static async Task<IActionResult> MakeReport(
        [HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("called MakeReport");
        return new OkObjectResult("ok");
    }
    [FunctionName("ReadReport")]
    public static async Task<IActionResult> ReadReport(
        [HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("called ReadReport");
        return new OkObjectResult("ok");
    }
}
#endif
}
