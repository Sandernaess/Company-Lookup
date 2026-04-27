namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public interface IEnhetService
    {
        Task<EnhetResponse?> GetEnhet(string orgnr, CancellationToken cancellationToken);
    }
}
