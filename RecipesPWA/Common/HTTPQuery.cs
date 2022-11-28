namespace RecipesPWA.Common
{
    /// <summary>
    /// Record to store HTTP query informations.
    /// </summary>
    public record HTTPQuery
    {
        /// <summary>
        /// Gets the name for the query.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the value for the query.
        /// </summary>
        public string Value { get; init; }
    }
}
