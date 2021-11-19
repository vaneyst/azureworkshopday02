using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace AzureWorkshop.DayTwo.Sender
{
    class Program
    {
        private static readonly string _connectionString = "";
        private static readonly string _topicName = "some-topic-name";

        static async Task Main(string[] args)
        {
            await EnsureTopicAsync();

            var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(_topicName);

            string message;
            while (!string.IsNullOrWhiteSpace(message = Console.ReadLine()))
            {
                await sender.SendMessageAsync(new ServiceBusMessage(message));
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task EnsureTopicAsync()
        {
            var client = new ServiceBusAdministrationClient(_connectionString);
            var exists = await client.TopicExistsAsync(_topicName);
            if (exists)
            {
                Console.WriteLine($"Topic {_topicName} already exists.");
            }
            else
            {
                try
                {
                    Console.WriteLine($"Creating topic {_topicName}...");
                    await client.CreateTopicAsync(_topicName);
                    Console.WriteLine($"Created topic {_topicName}.");
                }
                catch (Exception e) when (e.Message.Contains("409"))
                {
                    Console.WriteLine($"Topic {_topicName} was already created by another process.");
                }
            }
        }
    }
}