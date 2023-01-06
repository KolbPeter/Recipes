using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecipesFunctions.Common;
using RecipesFunctions.Common.MongoDb.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using RecipesFunctions.Common.MongoDb.Collections;

namespace RecipesFunctions
{
    public class GetByName
    {
        private readonly IMongoDbRepository<Recipe> _recipeRepository;

        public GetByName(IMongoDbRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [FunctionName("GetByName")]
        public async Task<IActionResult> Run(
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
                var recipes = await _recipeRepository.GetByAsync("Name", nameToSearch);
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
