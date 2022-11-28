using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.MongoDb.Repositories;

namespace RecipesFunctions
{
    public static class GetByName
    {
        [FunctionName("GetByName")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetByName")] HttpRequest req,
            ExecutionContext executionContext,
            ILogger log)
        {
            log.LogInformation($"{executionContext.FunctionName} processed a request.");

            string nameToSearch = req.Query["name"];

            dynamic data = JsonConvert.DeserializeObject(await new StreamReader(req.Body).ReadToEndAsync());
            nameToSearch = nameToSearch ?? data?.name ?? string.Empty;

            try
            {
                var recepies = await new RecipeRepository().GetByAsync("Name", nameToSearch);
                return new OkObjectResult(recepies);
            }
            catch (Exception ex)
            {
                log.LogError($"Get recipe by name failed with {ex.InnerstException()}");
                return new BadRequestErrorMessageResult($"Get recipe by name failed with {ex.InnerstException()}");
            }
        }
    }
}
