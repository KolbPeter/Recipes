using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.MongoDb.Collections
{
    public record Recipe : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Name { get; init; }
        public IEnumerable<string> Instructions { get; init; }
        public IEnumerable<SubRecipe> SubRecipes { get; init; }
    }
}
