namespace RecipesPWA.Common.Recipes
{
    /// <summary>
    /// Interface for objects with Ids.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the id of the object.
        /// </summary>
        public string Id { get; set; }
    }
}
