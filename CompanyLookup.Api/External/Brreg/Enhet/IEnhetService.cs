namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public interface IEnhetService
    {
        Task<EnhetResponse?> GetEnhet(string orgnr, CancellationToken cancellationToken);
        Task<IEnumerable<EnhetResponse>> SearchEnheterByName(string name, CancellationToken cancellationToken);
    }
}
