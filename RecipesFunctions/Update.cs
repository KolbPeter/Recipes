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
    public class Update
    {
        private readonly IMongoDbRepository<Recipe> _recipeRepository;

        public Update(IMongoDbRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [FunctionName("Update")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Update")] HttpRequest req,
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
                await _recipeRepository.UpdateAsync(recipe);
                return new OkObjectResult(recipe);
            }
            catch (Exception ex)
            {
                log.LogError($"Updating recipe failed: {ex.InnerstException()}");
                return new BadRequestErrorMessageResult($"Updating recipe failed: {ex.InnerstException()}");
            }
        }
    }
}
