using CompanyLookup.Api.External.Brreg;
using CompanyLookup.Api.External.Brreg.Enhet;
using CompanyLookup.Api.Services.Companies;

namespace CompanyLookup.Api.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();

            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IBrregApiClient, BrregApiClient>();
            services.AddScoped<IEnhetService, EnhetService>();
            
            return services;
        }
    }
}
