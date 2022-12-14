using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RecipesFunctions.Common;
using RecipesFunctions.Common.MongoDb.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using RecipesFunctions.Common.MongoDb.Collections;

namespace RecipesFunctions
{
    public class GetAll
    {
        private readonly IMongoDbRepository<Recipe> _recipeRepository;

        public GetAll(IMongoDbRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [FunctionName("GetAll")]
        public async Task<IActionResult> Run(
            [HttpTrigger( AuthorizationLevel.Function, "get", Route = "GetAll")] HttpRequest req,
            ExecutionContext executionContext,
            ILogger log)
        {
            log.LogInformation($"{executionContext.FunctionName} processed a request.");

            try
            {
                var recipes = await _recipeRepository.GetAllAsync();
                return new OkObjectResult(recipes);
            }
            catch (Exception ex)
            {
                log.LogError($"Request failed with: {ex.InnerstException()}");
                return new BadRequestErrorMessageResult(ex.InnerstException());
            }
        }
    }
}
