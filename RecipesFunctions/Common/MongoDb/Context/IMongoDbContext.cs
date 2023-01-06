using RecipesFunctions.Common.MongoDb.Collections;
using MongoDB.Driver;

namespace RecipesFunctions.Common.MongoDb.Context
{
    public interface IMongoDbContext
    {
        /// <summary>
        /// Gets all elements of a collection.
        /// </summary>
        IMongoCollection<Recipe> Recipes { get; }
    }
}