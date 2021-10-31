using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Diagnostics;

namespace Toy_CloudStorage
{
    public static class InsertEntity
    {
        [FunctionName("InsertEntity")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("ThroughputTest", Connection = "TableStorage")] CloudTable client,
            ILogger log)
        {
            var prtitionKey = Guid.NewGuid().ToString("N");
            var sw = new Stopwatch();
            sw.Start();

            foreach (var i in Enumerable.Range(1, 10001))
            {
                var entity = new Entity
                {
                    PartitionKey = prtitionKey,
                    RowKey = i.ToString("N")
                };
                await client.ExecuteAsync(
                    TableOperation.Insert(entity)
                );
            }
            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine(elapsed);
        }

        [FunctionName("FetchEntity")]
        public static async Task FetchAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("ThroughputTest", "TableStorage")] CloudTable client,
            ILogger log
        )
        {
            const string partitionKey = "17cd1cb109ca4086880ba52b5146a3f0";
            var sw = new Stopwatch();
            sw.Start();

            TableContinuationToken token = null;
            var query = new TableQuery<Entity>()
            .Where(
                TableQuery.GenerateFilterCondition(nameof(Entity.PartitionKey), QueryComparisons.Equal, partitionKey)
            );
            do
            {
                var result = await client.ExecuteQuerySegmentedAsync(query, token);
                token = result.ContinuationToken;
            } while (token != null);

            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine(elapsed);
        }

        private class Entity: TableEntity
        {
        }
    }
}
