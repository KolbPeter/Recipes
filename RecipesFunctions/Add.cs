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
    public static class Add
    {
        [FunctionName("Add")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Add")] HttpRequest req,
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
                var result = await new RecipeRepository().CreateAsync(recipe);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError($"Deserialization failed with {ex.InnerstException()}");
                return new BadRequestErrorMessageResult($"Adding recipe to database failed: {ex.InnerstException()}");
            }
        }
    }
}
