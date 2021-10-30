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
            
            foreach(var i in Enumerable.Range(1, 10001))
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
        }

        private class Entity: TableEntity
        {
        }
    }
}
