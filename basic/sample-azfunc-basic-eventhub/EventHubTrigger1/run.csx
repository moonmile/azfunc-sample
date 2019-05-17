using System;

public static void Run(string myEventHubMessage, ILogger log)
{
  log.LogInformation($"C# function triggered to process a message: {myEventHubMessage}");
}
