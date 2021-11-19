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

        static async Task Main(string[] args)
        {
            await EnsureTopicAsync();

            Console.WriteLine("Enter the subscription name:");

            var subscriptionName = await EnsureSubscriptionAsync(Console.ReadLine());

            var client = new ServiceBusClient(_connectionString);
            var receiver = client.CreateReceiver(_topicName, subscriptionName);

            ServiceBusReceivedMessage message;

            while ((message = await receiver.ReceiveMessageAsync()) != null)
            {
                if (message.ApplicationProperties.TryGetValue("Number", out var number))
                    Console.WriteLine($"{message.Body} - Number {number}");
                else
                    Console.WriteLine($"{message.Body} - Number not found.");
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

        private static async Task<string> EnsureSubscriptionAsync(string subscriptionName)
        {
            var client = new ServiceBusAdministrationClient(_connectionString);
            var exists = await client.SubscriptionExistsAsync(_topicName, subscriptionName);
            if (exists)
            {
                Console.WriteLine($"Subscription {subscriptionName} on topic {_topicName} already exists.");
            }
            else
            {
                try
                {
                    Console.WriteLine($"Creating subscription {subscriptionName} on topic {_topicName}...");
                    var options = new CreateSubscriptionOptions(_topicName, subscriptionName)
                    {

                    };
                    var rule = new CreateRuleOptions("rule", new SqlRuleFilter("Number % 2 = 0"));
                    await client.CreateSubscriptionAsync(options, rule);
                    Console.WriteLine($"Created subscription {subscriptionName} on topic {_topicName}.");
                }
                catch (Exception e) when (e.Message.Contains("409"))
                {
                    Console.WriteLine($"Subscription {subscriptionName} on topic {_topicName} was already created by another process.");
                }
            }

            return subscriptionName;
        }
    }
}
