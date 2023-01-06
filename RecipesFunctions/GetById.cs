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
    public class GetById
    {
        private readonly IMongoDbRepository<Recipe> _recipeRepository;

        public GetById(IMongoDbRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [FunctionName("GetById")]
        public async Task<IActionResult> Run(
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
                var recipes = (await _recipeRepository.GetById(idToSearch));
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
