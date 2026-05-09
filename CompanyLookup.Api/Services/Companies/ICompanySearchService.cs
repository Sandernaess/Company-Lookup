using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public interface ICompanySearchService
    {
        Task<IEnumerable<CompanyResponse>> SearchAsync(CompanySearchQuery query, CancellationToken cancellationToken);
    }
}
