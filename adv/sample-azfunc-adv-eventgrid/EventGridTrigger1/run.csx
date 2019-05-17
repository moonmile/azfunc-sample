#r "Microsoft.Azure.EventGrid"
using Microsoft.Azure.EventGrid.Models;

/// Event Grid トリガーを使った場合は、自動認証される。
public static void Run(EventGridEvent eventGridEvent, ILogger log)
{
    log.LogInformation(eventGridEvent.Data.ToString());
}
