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
        }

        private static async Task EnsureSubscriptionAsync()
        {
        }
    }
}
