using CompanyLookup.Api.External.Brreg;

namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public class EnhetService(IBrregApiClient brregApiClient) : IEnhetService
    {
        private readonly IBrregApiClient _apiClient = brregApiClient;

        public async Task<EnhetResponse?> GetEnhet(string orgnr, CancellationToken cancellationToken)
        {
            return await _apiClient.GetAsync<EnhetResponse>($"enheter/{orgnr}", cancellationToken);
        }
    }
}
