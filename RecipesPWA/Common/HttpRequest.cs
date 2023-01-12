namespace RecipesPWA.Common
{
    /// <summary>
    /// Record to store HTTP request message properties.
    /// </summary>
    public record HttpRequest
    {
        /// <summary>
        /// Gets the Url string for this request.
        /// </summary>
        public string Url { get; init; } = string.Empty;

        /// <summary>
        /// Gets the <see cref="HttpMethod"/> to use for this request.
        /// </summary>
        public HttpMethod Method { get; init; } = HttpMethod.Get;

        /// <summary>
        /// Gets the <see cref="StringContent"/> for this request.
        /// </summary>
        public StringContent? Content { get; init; } = null;

        /// <summary>
        /// Gets a collection of <see cref="RequestHeader"/> to use for this request.
        /// </summary>
        public IEnumerable<RequestHeader> Headers { get; init; } = Enumerable.Empty<RequestHeader>();
    }
}