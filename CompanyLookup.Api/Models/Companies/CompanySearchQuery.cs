namespace CompanyLookup.Api.Models.Companies
{
    public record CompanySearchQuery(
        string Name,
        int? Page = null,
        int? Size = null);
}
