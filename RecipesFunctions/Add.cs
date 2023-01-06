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
    public class Add
    {
        private readonly IMongoDbRepository<Recipe> _recipeRepository;

        public Add(IMongoDbRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [FunctionName("Add")]
        public async Task<IActionResult> Run(
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
                var result = await _recipeRepository.CreateAsync(recipe);
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
