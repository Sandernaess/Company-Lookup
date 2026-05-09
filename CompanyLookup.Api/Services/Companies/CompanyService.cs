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
            
            EnhetResponse? enhet = await service.GetEnhetAsync(orgnr, cancellationToken);
            if (enhet is null)
            {
                throw new Exception("Enhet not found in brreg");
            }

            return enhet.ToCompany();
        }
    }
}
