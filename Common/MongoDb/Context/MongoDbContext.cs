using MongoDB.Driver;
using Common.MongoDb.Collections;

namespace Common.MongoDb.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        //private const string connectionString = "mongodb://receptek:RnagH1zYhzte3gJVacsszlvclzVYWFw1i35TNo1uiGz8Xcks1d5xAfzVDGVV8FaZstoRYa4xAfcsSxPUNSv6DA%3D%3D@receptek.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@receptek@";
        private const string connectionString = "mongodb://kp-recipes:kC6nhOSRODXIA4aKAa5apUmZQ17fys7Sb51b4OXb65YWXzbKnbcnrSbFmyPvwLiEozqqGJWkVVrAuCTiC8lSpQ==@kp-recipes.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@kp-recipes@";
        private readonly IMongoDatabase database;

        /// <summary>
        /// Creates a Mongo db context.
        /// </summary>
        public MongoDbContext()
        {
            this.database = new MongoClient(connectionString).GetDatabase("Recipes");
        }

        /// <inheritdoc />
        public IMongoCollection<Recipe> Recipies =>
            database.GetCollection<Recipe>("Recipes");
    }
}
