using Microsoft.Extensions.Configuration;
using RecipesFunctions.Common.MongoDb.Collections;
using MongoDB.Driver;

namespace RecipesFunctions.Common.MongoDb.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase database;
        private readonly string _connectionString;

        /// <summary>
        /// Creates a Mongo db context.
        /// </summary>
        public MongoDbContext(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"];
            this.database = new MongoClient(_connectionString).GetDatabase("Recipes");
        }

        /// <inheritdoc />
        public IMongoCollection<Recipe> Recipes =>
            database.GetCollection<Recipe>("Recipes");
    }
}
