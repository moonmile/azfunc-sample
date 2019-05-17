using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.NotificationHubs;

namespace NotificationHubSample
{
public static class Function1
{
    [FunctionName("FuncPush")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, 
        "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("called FuncPush.");
        // POSTデータからパラメーターを取得
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync(); 
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string name = data?.name;

        string message = data?.message;
        var connectionString = 
            System.Environment.GetEnvironmentVariable("NOTIFICATIONHUB_CONNECTION");
        var hub =
            NotificationHubClient.CreateClientFromConnectionString(
                connectionString,
                "sample-azfunc-notification");
        var toast = @"
<toast>
    <visual>
        <binding template=""ToastText01"">
            <text id=""1"">" +
                $"from {name} : {message}" + @"
            </text>
        </binding>
    </visual>
</toast>";
        var outcome = await hub.SendWindowsNativeNotificationAsync(toast);

        return new OkObjectResult($"send message : {message}");
    }
}
}
