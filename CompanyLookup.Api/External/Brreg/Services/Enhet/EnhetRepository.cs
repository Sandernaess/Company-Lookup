using CompanyLookup.Api.External.Brreg.Models.Enhet;

namespace CompanyLookup.Api.External.Brreg.Services.Enhet
{
    public class EnhetRepository(IBrregApiClient brregApiClient) : IEnhetRepository
    {
        private readonly IBrregApiClient _apiClient = brregApiClient;

        public async Task<EnhetResponse?> GetEnhetAsync(
            string orgnr, 
            CancellationToken cancellationToken)
        {
            return await _apiClient.GetAsync<EnhetResponse>(
                $"enheter/{orgnr}", 
                cancellationToken);
        }

        public async Task<IEnumerable<EnhetResponse>> SearchEnheterByNameAsync(
            EnhetSearchQuery query,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Name))
            {
                return [];
            }

            var name = query.Name.Trim();
            var encodedName = Uri.EscapeDataString(name);

            var endpoint = $"enheter?navn={encodedName}";

            if (query.Page.HasValue)
            {
                endpoint += $"&page={query.Page}";
            }

            if (query.Size.HasValue)
            {
                endpoint += $"&size={query.Size}";
            }

            var result = await _apiClient.GetAsync<EnhetSearchResponse>(
                endpoint,
                cancellationToken);

            return result?.Embedded?.Enheter ?? [];
        }
    }
}
