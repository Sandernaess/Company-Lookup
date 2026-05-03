using CompanyLookup.Api.Endpoints.Companies;
using CompanyLookup.Api.Models.Companies;

namespace CompanyLookup.Api.Endpoints
{
    public static class CompaniesEndpoints
    {
        public static void MapCompaniesEndpoints(this WebApplication app)
        {
            RouteGroupBuilder group = app.MapGroup("/companies").WithTags("Companies");

            group.MapGet("/{orgnr}", GetCompanyByOrgnr.Handle)
                .Produces<CompanyResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/search", GetSearchCompanies.Handle)
                .Produces<List<CompanyResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
