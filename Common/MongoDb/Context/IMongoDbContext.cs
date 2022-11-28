using MongoDB.Driver;
using Common.MongoDb.Collections;

namespace Common.MongoDb.Context
{
    public interface IMongoDbContext
    {
        /// <summary>
        /// Gets all elements of a collection.
        /// </summary>
        IMongoCollection<Recipe> Recipies { get; }
    }
}