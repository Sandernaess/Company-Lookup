using CompanyLookup.Api.Common;
using CompanyLookup.Api.Models.Companies;
using CompanyLookup.Api.Services.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyLookup.Api.Endpoints.Companies
{
    public static class GetSearchCompanies
    {
        public static async Task<IResult> Handle(
            [AsParameters] CompanySearchRequest request,
            [FromServices] ICompanySearchService service,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return TypedResults.BadRequest("Missing name.");
            }

            var name = request.Name.Trim();
            if (name.Length < 2)
            {
                return TypedResults.BadRequest("Name must be at least 2 characters.");
            }

            var query = new CompanySearchQuery(name, request.Page, request.Size);

            var result = await service.SearchAsync(query, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
