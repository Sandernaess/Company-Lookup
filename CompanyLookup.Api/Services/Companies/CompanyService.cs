using CompanyLookup.Api.Common;
using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Mapping.Companies;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public class CompanyService(IEnhetRepository repository) : ICompanyService
    {
        public async Task<Result<CompanyResponse>> GetAsync(string orgnr, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(orgnr))
            {
                return Result.Failure<CompanyResponse>(
                    "Organization number cannot be null or empty.",
                    ErrorType.Validation);
            }

            EnhetResponse? enhet = await repository.GetEnhetAsync(orgnr, cancellationToken);
            if (enhet is null)
            {
                return Result.Failure<CompanyResponse>(
                    $"Company with orgnr {orgnr} not found.",
                    ErrorType.NotFound);
            }

            return Result.Success(enhet.ToCompany());
        }
    }
}
