using CompanyLookup.Api.Services.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CompanyLookup.Api.Endpoints.Companies
{
    public static class GetSearchCompaniesByName
    {
        public static async Task<IResult> Handle(
            [FromQuery] string name,
            [FromQuery] int page,
            [FromQuery] int size,
            [FromServices] ICompanyService service,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Results.BadRequest("Missing name.");
                }

                name = name.Trim();

                if (name.Length < 2) // Just to avoid unecessary calls to the service with very short names that are unlikely to yield useful results
                {
                    return Results.BadRequest("Name must be at least 2 characters.");
                }

                if (page < 0)
                {
                    return Results.BadRequest("Page must be 0 or greater.");
                }

                if (size < 10)
                {
                    return Results.BadRequest("Size must be greater than 10.");
                }

                if (size > 20)
                {
                    return Results.BadRequest("Size must be less than 20.");
                }

                var companies = await service.SearchAsync(name, page, size, cancellationToken);

                return Results.Ok(companies);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Unknown error occured when searching for companies with name: {name} - {ex.Message}";
                Console.WriteLine(errorMsg); // TODO: Log to a logging framework

                return Results.InternalServerError();
            }
        }
    }
}
