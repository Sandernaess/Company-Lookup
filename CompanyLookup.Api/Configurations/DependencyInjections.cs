using CompanyLookup.Api.External.Brreg;
using CompanyLookup.Api.External.Brreg.Services.Enhet;
using CompanyLookup.Api.Services.Companies;

namespace CompanyLookup.Api.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanySearchService, CompanySearchService>();

            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IBrregApiClient, BrregApiClient>();
            services.AddScoped<IEnhetRepository, EnhetRepository>();
            
            return services;
        }
    }
}
