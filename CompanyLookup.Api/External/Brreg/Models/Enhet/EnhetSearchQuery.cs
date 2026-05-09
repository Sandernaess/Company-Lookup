namespace CompanyLookup.Api.External.Brreg.Models.Enhet
{
    public record EnhetSearchQuery(
        string Name,
        int? Page = null,
        int? Size = null);
}
