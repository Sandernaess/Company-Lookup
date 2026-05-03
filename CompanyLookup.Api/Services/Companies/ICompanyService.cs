using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Services.Companies
{
    public interface ICompanyService
    {
        Task<CompanyResponse> GetAsync(string orgnr, CancellationToken cancellationToken);

        Task<IEnumerable<CompanyResponse>> SearchAsync(string name, CancellationToken cancellationToken);
    }
}
