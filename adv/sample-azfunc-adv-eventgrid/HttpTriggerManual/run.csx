#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

/// 手動応答をする場合は、validationUrl の値を抜き出して、
/// 5分以内にブラウザでURLを呼び出すと認証される。

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("called HttpTriggerManual");
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    string validationUrl = data[0]?.data.validationUrl;
    log.LogInformation("validationUrl : " + validationUrl);
    return new OkObjectResult("ok");
}
