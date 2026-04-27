namespace CompanyLookup.Api.External.Brreg
{
    public interface IBrregApiClient
    {
        Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken);
    }
}
