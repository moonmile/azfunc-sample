using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionAppEventHub
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run(
            [EventHubTrigger("samples-workitems", 
            Connection = "EVENTHUB_CONNECTION")]
            string myEventHubMessage, ILogger log)
        {
            log.LogInformation($"C# function triggered to process a message: {myEventHubMessage}");
        }
    }
}
