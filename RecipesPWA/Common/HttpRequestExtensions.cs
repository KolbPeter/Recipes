using System.Text.Json;

namespace RecipesPWA.Common
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequest"/>
    /// </summary>
    public static class HttpRequestExtensions
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Sets the <see cref="HttpMethod"/> for this <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
        /// <param name="method">The <see cref="HttpMethod"/> to set.</param>
        /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
        public static HttpRequest SetMethod(this HttpRequest httpRequest, HttpMethod method) =>
            httpRequest with { Method = method };

        /// <summary>
        /// Sets the Url string for this <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
        /// <param name="url">The url string to set.</param>
        /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
        public static HttpRequest SetUrlString(this HttpRequest httpRequest, string url) =>
            httpRequest with { Url = url };

        /// <summary>
        /// Sets the <see cref="StringContent"/> for this <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
        /// <param name="content">The <see cref="StringContent"/> to set.</param>
        /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
        public static HttpRequest SetContent(this HttpRequest httpRequest, StringContent content) =>
            httpRequest with { Content = content };

        /// <summary>
        /// Adds the <see cref="RequestHeader"/> for this <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
        /// <param name="requestHeader">The <paramref name="requestHeader"/> to add.</param>
        /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
        public static HttpRequest AddHeader(this HttpRequest httpRequest, RequestHeader requestHeader) =>
            httpRequest with { Headers = httpRequest.Headers.Append(requestHeader) };

        /// <summary>
        /// Adds the given <paramref name="requestHeaders"/> for this <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
        /// <param name="requestHeaders">The <paramref name="requestHeaders"/> to add.</param>
        /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
        public static HttpRequest AddHeaders(this HttpRequest httpRequest, IEnumerable<RequestHeader> requestHeaders) =>
            requestHeaders.Aggregate(httpRequest, (current, requestHeader) => current.AddHeader(requestHeader));

        /// <summary>
        /// Awaitable. Sends the given <paramref name="httpRequest"/> to the given <see cref="HttpClient"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected result object in the result context.</typeparam>
        /// <param name="httpRequest">The <see cref="HttpRequest"/> to send.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> to send the request for.</param>
        /// <param name="defaultReturnValue">The default return value to send if the result is null.</param>
        /// <returns>Returns a <typeparam name="T"> object or the <paramref name="defaultReturnValue"/> if the result is null.</typeparam></returns>
        public static async Task<T> SendRequestAsync<T>(
            this HttpRequest httpRequest,
            HttpClient httpClient,
            T defaultReturnValue) =>
                JsonSerializer
                    .Deserialize<T>(
                        json: await httpRequest.WaitResponseStringAsync(httpClient),
                        options: _serializerOptions)
                ?? defaultReturnValue;

        private static async Task<string> WaitResponseStringAsync(this HttpRequest httpRequest, HttpClient httpClient)
        {
            var result = await httpClient
                .SendAsync(httpRequest.ConvertToHttpRequestMessage());

            return await result
                .Content
                .ReadAsStringAsync();
        }

        private static HttpRequestMessage ConvertToHttpRequestMessage(this HttpRequest httpRequest)
        {
            var result = new HttpRequestMessage
            {
                Method = httpRequest.Method,
                RequestUri = new Uri(httpRequest.Url),
                Content = httpRequest.Content
            };

            foreach (var httpRequestHeader in httpRequest.Headers)
            {
                result.Headers.Add(name: httpRequestHeader.Name, value: httpRequestHeader.Value);
            }

            return result;
        }
    }
}
