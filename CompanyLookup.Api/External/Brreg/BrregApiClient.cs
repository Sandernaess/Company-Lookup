namespace CompanyLookup.Api.External.Brreg
{
    public class BrregApiClient(IHttpClientFactory httpClientFactory, IConfiguration config) : IBrregApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _baseUrl = GetValidatedBaseUrl(config.GetValue<string>("BrregApiUrl"));

        private static string GetValidatedBaseUrl(string? baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("Missing BrregApiUrl configuration value.");

            return NormalizeUrl(baseUrl);
        }

        private static string NormalizeUrl(string url)
        {
            return url.EndsWith("/") ? url : url + "/";
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl);

            return client;
        }

        public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            var response = await CreateClient().GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        }
    }
}
