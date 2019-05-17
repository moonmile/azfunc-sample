using System;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEventRead
{
    class Program
    {
        // イベントプロセッサホストを利用した場合
        // refer https://docs.microsoft.com/ja-jp/azure/event-hubs/event-hubs-dotnet-standard-getstarted-receive-eph
        // 
        private const string EventHubConnectionString = "";
        private const string EventHubName = "samples-workitems";
        private const string StorageContainerName = "eventhub-items";
        private static readonly string StorageConnectionString = "";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");
            var eventProcessorHost = new EventProcessorHost(
                EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageContainerName);
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();
            Console.WriteLine("Any hit key.");
            Console.ReadLine();
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }


    public class SimpleEventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                Console.WriteLine($"Message received. Partition: '{context.PartitionId}', Data: '{data}'");
            }
            return context.CheckpointAsync();
        }
    }
}
