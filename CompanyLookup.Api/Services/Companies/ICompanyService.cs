using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public interface ICompanyService
    {
        Task<CompanyResponse> GetAsync(string orgnr, CancellationToken cancellationToken);
    }
}
