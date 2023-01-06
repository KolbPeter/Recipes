using RecipesFunctions.Common.MongoDb.Collections;
using RecipesFunctions.Common.MongoDb.Context;

namespace RecipesFunctions.Common.MongoDb.Repositories
{
    public class RecipeRepository : GenericMongoDbRepository<Recipe>
    {
        public RecipeRepository(IMongoDbContext mongoDbContext)
            : base(mongoDbContext.Recipes)
        {
        }
    }
}
