using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Extensions.Companies
{
    public static class CompanySearchRequestExtensions
    {
        public static CompanySearchQuery ToQuery(
            this CompanySearchRequest request)
        {
            return new CompanySearchQuery(
                request.Name,
                request.Page,
                request.Size);
        }
    }
}
