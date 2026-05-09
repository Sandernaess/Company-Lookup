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
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Results.BadRequest("Missing name.");
                }

                var name = request.Name.Trim();
                if (name.Length < 2)
                {
                    return Results.BadRequest("Name must be at least 2 characters.");
                }

                var page = request.Page;
                var size = request.Size;

                var query = new CompanySearchQuery(name, page, size);

                var companies = await service.SearchAsync(query, cancellationToken);

                return Results.Ok(companies);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Unknown error occured when searching for companies with name: {request.Name} - {ex.Message}";
                Console.WriteLine(errorMsg); // TODO: Log to a logging framework

                return Results.InternalServerError();
            }
        }
    }
}
