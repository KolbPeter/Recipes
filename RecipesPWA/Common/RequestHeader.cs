namespace RecipesPWA.Common
{
    /// <summary>
    /// Record to store a HTTP request header information.
    /// </summary>
    public record RequestHeader
    {
        /// <summary>
        /// Gets the name for the request header.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the value for the request header.
        /// </summary>
        public string Value { get; init; }
    }
}
