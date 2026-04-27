using CompanyLookup.Api.External.Brreg.Enhet;
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
                throw new ArgumentNullException(nameof(orgnr));
            }
            
            EnhetResponse? response = await service.GetEnhet(orgnr, cancellationToken);

            if (response is null)
            {
                throw new Exception("Enhet not found in brreg");
            }

            return response.ToCompany();
        }
    }
}
