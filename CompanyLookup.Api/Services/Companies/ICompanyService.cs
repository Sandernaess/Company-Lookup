using CompanyLookup.Api.Common;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public interface ICompanyService
    {
        Task<Result<CompanyResponse>> GetAsync(string orgnr, CancellationToken cancellationToken);
    }
}
