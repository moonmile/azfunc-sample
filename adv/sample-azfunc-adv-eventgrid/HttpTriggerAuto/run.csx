#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

/// Event Grid からの検証を、自分の webhook で行う場合は
/// コードから validationCode を抜き出し、応答を返す

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("called HttpTriggerAuto");
    var validationEventType = "Microsoft.EventGrid.SubscriptionValidationEvent";
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);

    if ( data[0]?.eventType == validationEventType ) {
        // 検証コードを自動で返す
        string validationCode = data[0]?.data.validationCode;
        log.LogInformation("validationCode : " + validationCode);
        return new OkObjectResult($"{{ \"ValidationResponse\": \"{validationCode}\" }}");
    }
    // ここは実際の処理
    log.LogInformation("your code in HttpTriggerAuto.");
    return new OkObjectResult("ok");

}
