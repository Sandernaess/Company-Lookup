using CompanyLookup.Api.Common;
using CompanyLookup.Api.External.Brreg.Models.Enhet;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Mapping.Companies;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public class CompanySearchService(IEnhetRepository repository) : ICompanySearchService
    {
        public async Task<Result<IEnumerable<CompanyResponse>>> SearchAsync(CompanySearchQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            if (string.IsNullOrWhiteSpace(query.Name))
            {
                return Result.Failure<IEnumerable<CompanyResponse>>(
                    "Name cannot be empty.",
                    ErrorType.Validation);
            }

            var enhetQuery = new EnhetSearchQuery(
                query.Name,
                query.Page,
                query.Size);

            IEnumerable<EnhetResponse> enheter = await repository.SearchEnheterByNameAsync(
                enhetQuery,
                cancellationToken);

            return Result.Success(enheter.Select(e => e.ToCompany()));
        }
    }
}
