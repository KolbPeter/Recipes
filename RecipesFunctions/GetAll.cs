using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.MongoDb.Repositories;

namespace RecipesFunctions
{
    public static class GetAll
    {
        [FunctionName("GetAll")]
        public static async Task<IActionResult> Run(
            [HttpTrigger( AuthorizationLevel.Function, "get", Route = "GetAll")] HttpRequest req,
            ExecutionContext executionContext,
            ILogger log)
        {
            log.LogInformation($"{executionContext.FunctionName} processed a request.");

            try
            {
                var recepies = await new RecipeRepository().GetAllAsync();
                return new OkObjectResult(recepies);
            }
            catch (Exception ex)
            {
                log.LogError($"Request failed with: {ex.InnerstException()}");
                return new BadRequestErrorMessageResult(ex.InnerstException());
            }
        }
    }
}
