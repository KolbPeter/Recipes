using System.Text.Json;

namespace RecipesPWA.Common
{
    /// <summary>
    /// Service to handle HTTP requests. Implementation of <see cref="IRequestService{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects to retrieve or send.</typeparam>
    public class RequestService<T> : IRequestService<T> where T : class, new()
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<RequestHeader> _defaultHeaders;
        private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Constructor for a <see cref="IRequestService{T}"/> implementation.
        /// </summary>
        /// <param name="configuration"></param>
        public RequestService(IConfiguration configuration)
        {
            _client = new HttpClient();
            _configuration = configuration;
            _defaultHeaders = _configuration
                .GetSection("RequestHeaders")
                .Get<IEnumerable<RequestHeader>>();
            _defaultHeaders ??= Enumerable.Empty<RequestHeader>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAll(IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetUrlString(UrlString("/api/GetAll"))
                .SendRequestAsync(
                    httpClient: _client,
                    defaultReturnValue: Enumerable.Empty<T>());
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByName(
            string name,
            IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetUrlString(UrlString("/api/GetByName", new Dictionary<string, string> { { "Name", name } }))
                .SendRequestAsync(
                    httpClient: _client,
                    defaultReturnValue: Enumerable.Empty<T>());
        }

        /// <inheritdoc/>
        public async Task<T?> GetById(
            string id,
            IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetUrlString(UrlString("/api/GetById", new Dictionary<string, string> { { "Id", id } }))
                .SendRequestAsync<T?>(
                    httpClient: _client,
                    defaultReturnValue: default);
        }

        /// <inheritdoc/>
        public async Task<T> Add(T newObject, IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetMethod(HttpMethod.Post)
                .SetContent(new StringContent(JsonSerializer.Serialize(newObject, _serializerOptions)))
                .SetUrlString(UrlString("/api/Add"))
                .SendRequestAsync(
                    httpClient: _client,
                    defaultReturnValue: new T());
        }

        /// <inheritdoc/>
        public async Task<T> Update(T objectToUpdate, IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetMethod(HttpMethod.Put)
                .SetContent(new StringContent(JsonSerializer.Serialize(objectToUpdate, _serializerOptions)))
                .SetUrlString(UrlString("/api/Update"))
                .SendRequestAsync(
                    httpClient: _client,
                    defaultReturnValue: new T());
        }

        /// <inheritdoc/>
        public async Task<T> Remove(T objectToDelete, IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetMethod(HttpMethod.Delete)
                .SetContent(new StringContent(JsonSerializer.Serialize(objectToDelete, _serializerOptions)))
                .SetUrlString(UrlString("/api/Remove"))
                .SendRequestAsync(
                    httpClient: _client,
                    defaultReturnValue: new T());
        }

        private string UrlString(string route, IDictionary<string, string>? queries = null) =>
            $"{_configuration["BaseUrl"]}{route}{QueryString(queries)}";

        private string QueryString(IDictionary<string, string>? queries) =>
            queries != null
                ? $"?{string.Join("&", queries.Select(x => $"{x.Key}={x.Value}").ToArray())}"
                : string.Empty;
    }
}