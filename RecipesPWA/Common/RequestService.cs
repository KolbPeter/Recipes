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
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

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
                .SetUrlString(UrlString("/api/GetByName", new[] { new HTTPQuery() { Name = "Name", Value = name } }))
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
                .SetUrlString(UrlString("/api/GetById", new[] { new HTTPQuery() { Name = "Id", Value = id } }))
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
                .SetContent(new StringContent(JsonSerializer.Serialize(newObject)))
                .SetUrlString(UrlString("/api/Add"))
                .SendRequestAsync<T>(
                    httpClient: _client,
                    defaultReturnValue: new T());
        }

        /// <inheritdoc/>
        public async Task<T> Update(T objectToUpdate, IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetMethod(HttpMethod.Put)
                .SetContent(new StringContent(JsonSerializer.Serialize(objectToUpdate)))
                .SetUrlString(UrlString("/api/Update"))
                .SendRequestAsync<T>(
                    httpClient: _client,
                    defaultReturnValue: new T());
        }

        /// <inheritdoc/>
        public async Task<T> Remove(T objectToDelete, IEnumerable<RequestHeader>? headers = null)
        {
            return await new HttpRequest()
                .AddHeaders(_defaultHeaders)
                .SetMethod(HttpMethod.Delete)
                .SetContent(new StringContent(JsonSerializer.Serialize(objectToDelete)))
                .SetUrlString(UrlString("/api/Remove"))
                .SendRequestAsync<T>(
                    httpClient: _client,
                    defaultReturnValue: new T());
        }

        private string UrlString(string route, IEnumerable<HTTPQuery>? queries = null) =>
            $"{_configuration["BasicUrl"]}{route}{QueryString(queries)}";

        private string QueryString(IEnumerable<HTTPQuery>? queries) =>
            queries != null
            ? $"?{string.Join("&", queries.Select(x => $"{x.Name}={x.Value}").ToArray())}"
            : string.Empty;

        private void DefaultHeaders()
        {
            _client.DefaultRequestHeaders.Clear();
            AddHeaders(_defaultHeaders);
        }

        private void AddHeader(RequestHeader header)
        {
            _client.DefaultRequestHeaders.Add(header.Name, header.Value);
        }

        private void AddHeaders(IEnumerable<RequestHeader>? headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    AddHeader(header);
                }
            }
        }
    }
}
