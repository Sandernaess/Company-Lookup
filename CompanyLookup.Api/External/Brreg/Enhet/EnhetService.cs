namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public class EnhetService(IBrregApiClient brregApiClient) : IEnhetService
    {
        private readonly IBrregApiClient _apiClient = brregApiClient;

        private const int DefaultPage = 1;
        private const int DefaultSize = 10;
        private const int MaxSize = 20;

        public async Task<EnhetResponse?> GetEnhet(
            string orgnr, 
            CancellationToken cancellationToken)
        {
            return await _apiClient.GetAsync<EnhetResponse>(
                $"enheter/{orgnr}", 
                cancellationToken);
        }

        public async Task<IEnumerable<EnhetResponse>> SearchEnheterByName(
            EnhetSearchQuery query, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Name))
            {
                return [];
            }

            var name = query.Name.Trim();

            var encodedName = Uri.EscapeDataString(name);

            var page = Math.Max(query.Page, DefaultPage);
            var size = Math.Clamp(query.Size, DefaultSize, MaxSize);

            var endpoint =
                $"enheter?navn={encodedName}&page={page}&size={size}";

            var result = await _apiClient.GetAsync<EnhetSearchResponse>(
                endpoint,
                cancellationToken);

            return result?.Embedded?.Enheter ?? [];
        }
    }
}
