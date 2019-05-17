#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

/// これはレスポンスを返さないのでエラーになる。
/// イベントサブスクリプションは10分程度で自動削除されるはず。
public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");
    return new OkObjectResult("ok");
}
