namespace RecipesFunctions.Common.MongoDb.Collections
{
    public record Ingredient
    {
        public string Name { get; init; }
        public string Unit { get; init; }
        public decimal Quantity { get; init; }
    }
}
