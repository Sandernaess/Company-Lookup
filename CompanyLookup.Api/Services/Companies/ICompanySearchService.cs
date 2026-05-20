using CompanyLookup.Api.Common;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public interface ICompanySearchService
    {
        Task<Result<IEnumerable<CompanyResponse>>> SearchAsync(CompanySearchQuery query, CancellationToken cancellationToken);
    }
}
