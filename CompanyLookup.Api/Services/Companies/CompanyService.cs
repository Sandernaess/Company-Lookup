using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Mapping.Companies;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public class CompanyService(IEnhetService service) : ICompanyService
    {
        public async Task<CompanyResponse> GetAsync(string orgnr, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(orgnr))
            {
                throw new ArgumentException("Organization number cannot be empty.", nameof(orgnr));
            }
            
            EnhetResponse? response = await service.GetEnhetAsync(orgnr, cancellationToken);
            if (response is null)
            {
                throw new Exception("Enhet not found in brreg");
            }

            return response.ToCompany();
        }

        public async Task<IEnumerable<CompanyResponse>> SearchAsync(string name, int page, int size, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            }

            var query = new EnhetSearchQuery(name, page, size);

            IEnumerable<EnhetResponse> response = await service.SearchEnheterByNameAsync(
                query, 
                cancellationToken);

            if (response is null || !response.Any())
            {
                return [];
            }

            return response.Select(r => r.ToCompany());
        }
    }
}
