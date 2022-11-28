using System.Collections.Generic;

namespace Common.MongoDb.Collections
{
    public record SubRecipe
    {
        public string Name { get; init; }
        public string Preparation { get; init; }
        public IEnumerable<Ingredient> Ingredients { get; init; }
    }
}
