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
using Common.MongoDb.Collections;
using Common.MongoDb.Repositories;

namespace RecipesFunctions
{
    public static class Remove
    {
        [FunctionName("Remove")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Remove")] HttpRequest req,
            ExecutionContext executionContext,
            ILogger log)
        {
            log.LogInformation($"{executionContext.FunctionName} processed a request.");

            Recipe recipe;

            try
            {
                recipe = JsonConvert.DeserializeObject<Recipe>(await new StreamReader(req.Body).ReadToEndAsync());
            }
            catch (Exception ex)
            {
                log.LogError($"Deserialization failed with {ex.InnerstException()}");
                return new BadRequestErrorMessageResult($"Deserialization failed with {ex.InnerstException()}");
            }

            try
            {
                var result = await new RecipeRepository().RemoveAsync(recipe);
                if (result.IsAcknowledged && result.DeletedCount == 1)
                {
                    return new OkObjectResult(recipe);
                }

                throw new Exception(
                    $"Result acknowledged: {result.IsAcknowledged} and the deleted count is {result.DeletedCount}!");
            }
            catch (Exception ex)
            {
                log.LogError($"Removing recipe failed: {ex.InnerstException()}");
                return new BadRequestErrorMessageResult($"Removing recipe failed: {ex.InnerstException()}");
            }
        }
    }
}
