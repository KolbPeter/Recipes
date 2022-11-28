using Common.MongoDb.Collections;
using Common.MongoDb.Context;

namespace Common.MongoDb.Repositories
{
    public class RecipeRepository : GenericMongoDbRepository<Recipe>
    {
        public RecipeRepository() : base(new MongoDbContext().Recipies)
        {
        }
    }
}
