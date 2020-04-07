using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryForDocument().Wait();
        }

        private async static Task QueryForDocument() {
            var config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

            var endpoint = config["CosmosEndpoint"];
            var masterKey = config["CosmosMasterKey"];

            using (var client = new CosmosClient(endpoint, masterKey))
            {
                var container = client.GetContainer("tododb", "todo");
                var sql = "Select * from c";
                var iterator = container.GetItemQueryIterator<dynamic>(sql);
                var page = await iterator.ReadNextAsync();

                foreach (var item in page)
                {
                    Console.WriteLine($"Todo { item.id } has { item.childern.count } childern");
                }

                Console.ReadLine();
            }
        }
    }
}
