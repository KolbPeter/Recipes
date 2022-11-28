﻿using System.Text.Json;

namespace RecipesPWA.Common
{
    /// <summary>
    /// Service to handle HTTP requests. Implementation of <see cref="IRequestService{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects to retrieve or send.</typeparam>
    public class RequestService<T> : IRequestService<T> where T : class
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<RequestHeader>? _defaultHeaders;
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

        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAll(IEnumerable<RequestHeader>? headers = null)
        {
            DefaultHeaders();
            var url = UrlString("/api/GetAll");
            var httpResponseMessage = await _client.GetAsync(url);
            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<T>>(contentString, _serializerOptions) ?? Enumerable.Empty<T>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByName(
            string name,
            IEnumerable<RequestHeader>? headers = null)
        {
            DefaultHeaders();
            var url = UrlString("/api/GetByName", new[] { new HTTPQuery() { Name = "Name", Value = name } });
            var httpResponseMessage = await _client.GetAsync(url);
            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T[]>(contentString, _serializerOptions) ?? Array.Empty<T>();
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
