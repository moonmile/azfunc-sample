﻿using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleIoTRead
{
    /// <summary>
    /// このクラスは複雑なので後で整理する
    /// </summary>
    class Program
    {
        class ReadDeviceToCloudMessages
        {
            // Event Hub-compatible endpoint
            // az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {your IoT Hub name}
            private readonly static string s_eventHubsCompatibleEndpoint = "";

            // Event Hub-compatible name
            // az iot hub show --query properties.eventHubEndpoints.events.path --name {your IoT Hub name}
            private readonly static string s_eventHubsCompatiblePath = "";

            // az iot hub policy show --name iothubowner --query primaryKey --hub-name {your IoT Hub name}
            private readonly static string s_iotHubSasKey = "";
            private readonly static string s_iotHubSasKeyName = "iothubowner";
            private static EventHubClient s_eventHubClient;

            // Asynchronously create a PartitionReceiver for a partition and then start 
            // reading any messages sent from the simulated client.
            private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
            {
                // Create the receiver using the default consumer group.
                // For the purposes of this sample, read only messages sent since 
                // the time the receiver is created. Typically, you don't want to skip any messages.
                var eventHubReceiver = s_eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
                Console.WriteLine("Create receiver on partition: " + partition);
                while (true)
                {
                    if (ct.IsCancellationRequested) break;
                    Console.WriteLine("Listening for messages on: " + partition);
                    // Check for EventData - this methods times out if there is nothing to retrieve.
                    var events = await eventHubReceiver.ReceiveAsync(100);

                    // If there is data in the batch, process it.
                    if (events == null) continue;

                    foreach (EventData eventData in events)
                    {
                        string data = Encoding.UTF8.GetString(eventData.Body.Array);
                        Console.WriteLine("Message received on partition {0}:", partition);
                        Console.WriteLine("  {0}:", data);
                        Console.WriteLine("Application properties (set by device):");
                        foreach (var prop in eventData.Properties)
                        {
                            Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
                        }
                        Console.WriteLine("System properties (set by IoT Hub):");
                        foreach (var prop in eventData.SystemProperties)
                        {
                            Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
                        }
                    }
                }
            }

            private static async Task Main(string[] args)
            {
                Console.WriteLine("IoT Hub Quickstarts - Read device to cloud messages. Ctrl-C to exit.\n");

                // Create an EventHubClient instance to connect to the
                // IoT Hub Event Hubs-compatible endpoint.
                // var connectionString = new EventHubsConnectionStringBuilder(new Uri(s_eventHubsCompatibleEndpoint), s_eventHubsCompatiblePath, s_iotHubSasKeyName, s_iotHubSasKey);
                var connectionString = "Endpoint=sb://ihsuprodkwres029dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=biFyL+1KE5LL7et4ax9XjX1jPuzUqA6x7PKG8n6uuj4=;EntityPath=iothub-ehub-sample-azf-1465379-e172db1ac3";
                s_eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());

                // Create a PartitionReciever for each partition on the hub.
                var runtimeInfo = await s_eventHubClient.GetRuntimeInformationAsync();
                var d2cPartitions = runtimeInfo.PartitionIds;

                CancellationTokenSource cts = new CancellationTokenSource();

                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                    Console.WriteLine("Exiting...");
                };

                var tasks = new List<Task>();
                foreach (string partition in d2cPartitions)
                {
                    tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
                }

                // Wait for all the PartitionReceivers to finsih.
                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}

