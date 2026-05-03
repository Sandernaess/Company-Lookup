using CompanyLookup.Api.External.Brreg.Models.Enhet;

namespace CompanyLookup.Api.External.Brreg.Services.Enhet
{
    public interface IEnhetService
    {
        Task<EnhetResponse?> GetEnhet(string orgnr, CancellationToken cancellationToken);
        Task<IEnumerable<EnhetResponse>> SearchEnheterByName(
            EnhetSearchQuery query, 
            CancellationToken cancellationToken);
    }
}
