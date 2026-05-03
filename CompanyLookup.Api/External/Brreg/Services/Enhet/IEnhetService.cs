using CompanyLookup.Api.External.Brreg.Models.Enhet;

namespace CompanyLookup.Api.External.Brreg.Services.Enhet
{
    public interface IEnhetService
    {
        Task<EnhetResponse?> GetEnhetAsync(string orgnr, CancellationToken cancellationToken);
        Task<IEnumerable<EnhetResponse>> SearchEnheterByNameAsync(
            EnhetSearchQuery query, 
            CancellationToken cancellationToken);
    }
}
