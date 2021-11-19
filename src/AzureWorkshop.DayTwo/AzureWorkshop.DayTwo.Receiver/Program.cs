using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace AzureWorkshop.DayTwo.Receiver
{
    class Program
    {
        private static readonly string _connectionString = "";
        private static readonly string _topicName = "some-topic-name";
        private static readonly string _subscriptionName = Environment.MachineName;

        static async Task Main(string[] args)
        {
            await EnsureTopicAsync();
            await EnsureSubscriptionAsync();

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

        private static async Task EnsureSubscriptionAsync()
        {
            var client = new ServiceBusAdministrationClient(_connectionString);
            var exists = await client.SubscriptionExistsAsync(_topicName, _subscriptionName);
            if (exists)
            {
                Console.WriteLine($"Subscription {_subscriptionName} on topic {_topicName} already exists.");
            }
            else
            {
                try
                {
                    Console.WriteLine($"Creating subscription {_subscriptionName} on topic {_topicName}...");
                    await client.CreateSubscriptionAsync(_topicName, _subscriptionName);
                    Console.WriteLine($"Created subscription {_subscriptionName} on topic {_topicName}.");
                }
                catch (Exception e) when (e.Message.Contains("409"))
                {
                    Console.WriteLine($"Subscription {_subscriptionName} on topic {_topicName} was already created by another process.");
                }
            }
        }
    }
}
