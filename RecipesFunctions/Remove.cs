using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecipesFunctions.Common;
using RecipesFunctions.Common.MongoDb.Collections;
using RecipesFunctions.Common.MongoDb.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace RecipesFunctions
{
    public class Remove
    {
        private readonly IMongoDbRepository<Recipe> _recipeRepository;

        public Remove(IMongoDbRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [FunctionName("Remove")]
        public async Task<IActionResult> Run(
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
                var result = await _recipeRepository.RemoveAsync(recipe);
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
