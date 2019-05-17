using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionAppCosmosDB
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "SampleDB",
            collectionName: "SampleCollection",
            ConnectionStringSetting = "COSMOSDB_CONNECTION",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);

                var doc = input[0];
                var name = doc.GetPropertyValue<string>("Name");
                var msg = doc.GetPropertyValue<string>("Message");
                log.LogInformation($"{name} : {msg}");
            }
        }
    }
}
