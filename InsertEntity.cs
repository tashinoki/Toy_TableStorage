using System;
using System.IO;
using System.Threading.Tasks;
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
            [Table("ThroughputTest", Connection = "TableStorage")] CloudTable configTable,
            ILogger log)
        {
        }
    }
}
