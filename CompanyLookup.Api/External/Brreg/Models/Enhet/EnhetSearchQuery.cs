namespace CompanyLookup.Api.External.Brreg.Models.Enhet
{
    public record EnhetSearchQuery(
        string Name,
        int Page = 1,
        int Size = 10);
}
