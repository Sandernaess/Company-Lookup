using CompanyLookup.Api.Models.Companies;
using CompanyLookup.Api.Services.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyLookup.Api.Endpoints.Companies
{
    public static class GetSearchCompanies
    {
        public static async Task<IResult> Handle(
             [AsParameters] CompanySearchRequest request,
            [FromServices] ICompanyService service,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Results.BadRequest("Missing name.");
                }

                var name = request.Name.Trim();
                if (name.Length < 2) // Just to avoid unecessary calls to the service with very short names that are unlikely to yield useful results
                {
                    return Results.BadRequest("Name must be at least 2 characters.");
                }

                var page = request.Page;
                if (page < 0)
                {
                    return Results.BadRequest("Page must be 0 or greater.");
                }

                var size = request.Size;
                if (size < 10 || size > 20)
                {
                    return Results.BadRequest("Size must be between 10 and 20.");
                }

                var companies = await service.SearchAsync(name, page, size, cancellationToken);

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
