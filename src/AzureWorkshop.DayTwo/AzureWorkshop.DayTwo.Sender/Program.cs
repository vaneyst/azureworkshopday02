using System;
using System.Threading.Tasks;

namespace AzureWorkshop.DayTwo.Sender
{
    class Program
    {
        private static readonly string _connectionString = "";
        private static readonly string _topicName = "some-topic-name";

        static async Task Main(string[] args)
        {
            await EnsureTopicAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task EnsureTopicAsync()
        {
        }
    }
}
