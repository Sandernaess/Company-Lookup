using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Mapping.Companies;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public class CompanySearchService(IEnhetService service) : ICompanySearchService
    {
        public async Task<IEnumerable<CompanyResponse>> SearchAsync(CompanySearchQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(query));
            }

            var enhetQuery = new EnhetSearchQuery(
                query.Name, 
                query.Page, 
                query.Size);

            IEnumerable<EnhetResponse> enheter = await service.SearchEnheterByNameAsync(
                enhetQuery, 
                cancellationToken);

            if (enheter is null || !enheter.Any())
            {
                return [];
            }

            return enheter.Select(r => r.ToCompany());
        }
    }
}
