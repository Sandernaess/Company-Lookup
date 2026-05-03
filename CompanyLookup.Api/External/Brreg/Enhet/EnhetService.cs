namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public class EnhetService(IBrregApiClient brregApiClient) : IEnhetService
    {
        private readonly IBrregApiClient _apiClient = brregApiClient;

        public async Task<EnhetResponse?> GetEnhet(string orgnr, CancellationToken cancellationToken)
        {
            return await _apiClient.GetAsync<EnhetResponse>(
                $"enheter/{orgnr}", 
                cancellationToken);
        }

        public async Task<IEnumerable<EnhetResponse>> SearchEnheterByName(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return [];
            }

            name = name.Trim();

            var encodedName = Uri.EscapeDataString(name);

            var result = await _apiClient.GetAsync<EnhetSearchResponse>(
                $"enheter?navn={encodedName}",
                cancellationToken);

            return result?.Embedded?.Enheter ?? [];
        }
    }
}
