using Common;
using Common.MongoDb.Repositories;
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

namespace RecipesFunctions
{
    public static class GetById

    {
        [FunctionName("GetById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetById")] HttpRequest req,
            ExecutionContext executionContext,
            ILogger log)
        {
            log.LogInformation($"{executionContext.FunctionName} processed a request.");

            string idToSearch = req.Query["id"];

            dynamic data = JsonConvert.DeserializeObject(await new StreamReader(req.Body).ReadToEndAsync());
            idToSearch = idToSearch ?? data?.id ?? string.Empty;

            try
            {
                var recipes = (await new RecipeRepository().GetById(idToSearch));
                return new OkObjectResult(recipes);
            }
            catch (Exception ex)
            {
                log.LogError($"Get recipe by name failed with {ex.InnerstException()}");
                return new BadRequestErrorMessageResult($"Get recipe by name failed with {ex.InnerstException()}");
            }
        }
    }
}
