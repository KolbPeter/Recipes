namespace RecipesPWA.Common
{
    /// <summary>
    /// Record to store information about a page.
    /// </summary>
    public record PageDescription
    {
        /// <summary>
        /// Gets the Uri of the page.
        /// </summary>
        public string Uri { get; init; }

        /// <summary>
        /// Gets a description for the page.
        /// </summary>
        public string Description { get; init; }
    }
}